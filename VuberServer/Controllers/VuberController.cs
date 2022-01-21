using System;
using System.Collections.Generic;
using System.Linq;
using Geolocation;
using Microsoft.AspNetCore.SignalR;
using VuberCore.Entities;
using VuberServer.Hubs;
using VuberCore.Dto;
using VuberServer.Clients;
using VuberServer.Data;
using VuberServer.Strategies.CalculateNewRatingStrategies;
using VuberServer.Strategies.CalculatePriceStrategies;
using VuberServer.Strategies.FindRidesWithLookingStatusStrategies;

namespace VuberServer.Controllers
{
    public class VuberController : IVuberController
    {
        private readonly IHubContext<ClientHub, IClientClient> _clientHubContext;
        private readonly IHubContext<DriverHub, IDriverClient> _driverHubContext;
        private readonly VuberDbContext _vuberDbContext;
        private WorkloadLevel WorkloadLevel;
        private decimal _maxLookingRidesForNormalWorkloadLevel;
        private ICalculateNewRatingStrategy _calculateNewRatingStrategy;
        private ICalculatePriceStrategy _calculatePriceStrategy;
        private IFindRidesWithLookingStatusStrategy _findRidesWithLookingStatusStrategy;

        public VuberController(
            IHubContext<ClientHub, IClientClient> clientHubContext,
            IHubContext<DriverHub, IDriverClient> driverHubContext,
            VuberDbContext vuberDbContext,
            ICalculateNewRatingStrategy calculateNewRatingStrategy,
            ICalculatePriceStrategy calculatePriceStrategy,
            IFindRidesWithLookingStatusStrategy findRidesWithLookingStatusStrategy)
        {
            _clientHubContext = clientHubContext ?? throw new ArgumentNullException(nameof(clientHubContext));
            _driverHubContext = driverHubContext ?? throw new ArgumentNullException(nameof(driverHubContext));
            _vuberDbContext = vuberDbContext;
            WorkloadLevel = WorkloadLevel.Normal;
            _calculateNewRatingStrategy = calculateNewRatingStrategy;
            _calculatePriceStrategy = calculatePriceStrategy;
            _findRidesWithLookingStatusStrategy = findRidesWithLookingStatusStrategy;
        }

        public Ride CreateNewRide(
            Guid clientId,
            Coordinate startLocation,
            ICollection<Coordinate> targetLocations,
            PaymentType paymentType,
            RideType rideType)
        {
            var clientForRide = _vuberDbContext.Clients.FirstOrDefault(client => client.Id == clientId) ??
                                throw new ArgumentNullException();
            var ride = new Ride()
            {
                Client = clientForRide,
                Cost = CalculatePrice(rideType, startLocation, targetLocations),
                PaymentType = paymentType,
                RideType = rideType,
                Status = RideStatus.Looking,
                StartLocation = startLocation,
                TargetLocations = targetLocations,
                Created = DateTime.UtcNow,
            };
            _vuberDbContext.Rides.Add(ride);
            _vuberDbContext.SaveChanges();
            var drivers = NearbyDrivers(startLocation, rideType);
            _driverHubContext.Clients.Clients(drivers.Select(driver => driver.Id.ToString()))
                .RideRequested(new RideToDriver(ride));
            return ride;
        }

        public bool DriverTakesRide(Guid driverId, Guid rideId)
        {
            var driverToTakeRide = _vuberDbContext.Drivers.FirstOrDefault(driver => driver.Id == driverId) ??
                                   throw new ArgumentNullException();
            var rideToTake = _vuberDbContext.Rides.FirstOrDefault(ride => ride.Id == rideId) ??
                             throw new ArgumentNullException();
            if (rideToTake.Status != RideStatus.Looking)
            {
                return false;
            }
            rideToTake.Driver = driverToTakeRide;
            rideToTake.Status = RideStatus.Waiting;
            rideToTake.Found = DateTime.UtcNow;
            _vuberDbContext.Rides.Update(rideToTake);
            _vuberDbContext.SaveChanges();
            _clientHubContext.Clients.User(rideToTake.Client.Id.ToString()).UpdateRide(new RideToClient(rideToTake));
            return true;
        }

        public void DriverArrives(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                             throw new ArgumentNullException();
            ride.Status = RideStatus.InProgress;
            ride.Started = DateTime.UtcNow;
            _vuberDbContext.Rides.Update(ride);
            _vuberDbContext.SaveChanges();
            _clientHubContext.Clients.User(ride.Client.Id.ToString()).UpdateRide(new RideToClient(ride));
        }

        public void RideCompleted(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                       throw new ArgumentNullException();
            WithdrawalForRide(ride);
            ride.Status = RideStatus.Complete;
            ride.Finished = DateTime.UtcNow;
            _vuberDbContext.Rides.Update(ride);
        }

        public void CancelRide(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                             throw new ArgumentNullException();
            switch (ride.Status)
            {
                case RideStatus.Looking:
                    ride.Status = RideStatus.Cancelled;
                    break;
                case RideStatus.Waiting:
                    ride.Status = RideStatus.Cancelled;
                    break;
                case RideStatus.InProgress:
                    ride.Status = RideStatus.Cancelled;
                    //снятие денег с клиента за часть поездки??
                    break;
                default:
                    throw new Exception("Ride cannot be cancelled");
            }
            ride.Finished = DateTime.UtcNow;
            _vuberDbContext.Rides.Update(ride);
            _driverHubContext.Clients.User(ride.Driver.Id.ToString()).RideCancelled();
        }

        private decimal CalculateRideLength(Coordinate startLocation, ICollection<Coordinate> targetLocations)
        {
            double length = Math.Sqrt(Math.Pow(startLocation.Latitude - targetLocations.First().Latitude, 2)
                                      + Math.Pow(startLocation.Longitude - targetLocations.First().Longitude, 2));
            if (targetLocations.Count > 1)
            {
                List<Coordinate> listOfTargetLocations = targetLocations.ToList();
                for (int i = 0; i < listOfTargetLocations.Count - 1; i++)
                {
                    Coordinate location1 = listOfTargetLocations[i];
                    Coordinate location2 = listOfTargetLocations[i + 1];
                    length += Math.Sqrt(Math.Pow(location1.Latitude - location2.Latitude, 2)
                                        + Math.Pow(location1.Longitude - location2.Longitude, 2));
                }
            }

            return Convert.ToDecimal(length);
        }

        private decimal CalculatePrice(RideType rideType, Coordinate startLocation, ICollection<Coordinate> targetLocations)
        {
            var rideLength = CalculateRideLength(startLocation, targetLocations);
            CheckWorkloadLevel();
            return _calculatePriceStrategy.CalculatePrice(rideLength, rideType, WorkloadLevel);
        }

        public List<Ride> SeeRides(Guid userId)
        {
            var rides = _vuberDbContext.Clients.FirstOrDefault(userToFind => userToFind.Id == userId).Rides ??
                        (_vuberDbContext.Drivers.FirstOrDefault(userToFind => userToFind.Id == userId).Rides  ??
                         throw new ArgumentNullException());

            return rides;
        }

        public void SetRating(Rating rating, Guid rideId, Func<Ride, User> userGetter)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(ride => ride.Id == rideId);
            var user = userGetter.Invoke(ride);
            _calculateNewRatingStrategy.CalculateNewRating(user.Rating, rating);
            _vuberDbContext.SaveChanges();
        }

        public void AddPaymentCard(Guid clientId, string cardData)
        {
            var client = _vuberDbContext.Clients.FirstOrDefault(clientToFind => clientToFind.Id == clientId) ??
                         throw new ArgumentNullException();
            client.PaymentCard = new PaymentCard() {CardData = cardData};
            _vuberDbContext.SaveChanges();
        }

        public void UpdateDriverLocation(Guid driverId, Coordinate location)
        {
            var driver = _vuberDbContext.Drivers.FirstOrDefault(driver => driver.Id == driverId);
            driver.LastKnownLocation = location;
            driver.LocationUpdatedAt = DateTime.UtcNow;
            _vuberDbContext.SaveChanges();
            var ride = _vuberDbContext.Rides.FirstOrDefault(ride =>
                ride.Driver.Id == driverId && ride.Status == RideStatus.Waiting);
            if (ride != null)
            {
                _clientHubContext.Clients.User(ride.Client.Id.ToString()).UpdateDriverLocation(location);
            }
        }

        private List<Ride> FindRidesWithLookingStatus()
        {
            return _findRidesWithLookingStatusStrategy.FindRidesWithLookingStatus(_vuberDbContext);
        }

        private void CheckWorkloadLevel()
        {
            WorkloadLevel = WorkloadLevel.Normal;
            if (_vuberDbContext
                .Rides
                .Where(ride => ride.Status == RideStatus.Looking).ToList().Count > _maxLookingRidesForNormalWorkloadLevel)
            {
                WorkloadLevel = WorkloadLevel.Loaded;
            }
        }

        private void WithdrawalForRide(Ride ride)
        {
            switch (ride.PaymentType)
            {
                case PaymentType.Cash:
                    //послать уведомление водителю, что с клиента нужно взять столько-то денег?? хз
                    break;
                case PaymentType.PaymentCard:
                    var paymentCard = ride.Client.PaymentCard ?? throw new ArgumentNullException();
                    //а что здесь вообще? можно либо при создании карты класть на нее рандомное кол-во денег, но я даже не знаю...
                    break;
            }
        }

        private IEnumerable<Driver> NearbyDrivers(Coordinate coordinate, RideType rideType)
        {
            return _vuberDbContext.Drivers.Where(driver =>
                GeoCalculator.GetDistance(driver.LastKnownLocation, coordinate, 1, DistanceUnit.Meters) < 1000 &&
                driver.MinRideLevel <= rideType && rideType <= driver.MaxRideLevel);
        }
    }
}