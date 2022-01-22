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
using VuberServer.Strategies.CheckWorkloadLevelStrategies;
using VuberServer.Strategies.CalculateRideDistanceStrategies;
using VuberServer.Strategies.FindRidesWithLookingStatusStrategies;
using VuberServer.Strategies.FindNearbyDriversStrategies;


namespace VuberServer.Controllers
{
    public class VuberController : IVuberController
    {
        private readonly IHubContext<ClientHub, IClientClient> _clientHubContext;
        private readonly IHubContext<DriverHub, IDriverClient> _driverHubContext;
        private readonly VuberDbContext _vuberDbContext;
        private WorkloadLevel WorkloadLevel;
        private ICalculateNewRatingStrategy _calculateNewRatingStrategy;
        private ICalculatePriceStrategy _calculatePriceStrategy;
        private ICheckWorkloadLevelStrategy _checkWorkloadLevelStrategy;
        private IFindRidesWithLookingStatusStrategy _findRidesWithLookingStatusStrategy;
        private ICalculateRideDistanceStrategy _calculateRideDistanceStrategy;
        private ICalculateLengthStrategy _calculateLengthStrategy;
        private IFindNearbyDriversStrategy _findNearbyDriversStrategy;

        public VuberController(
            IHubContext<ClientHub, IClientClient> clientHubContext,
            IHubContext<DriverHub, IDriverClient> driverHubContext,
            VuberDbContext vuberDbContext,
            ICalculateNewRatingStrategy calculateNewRatingStrategy,
            ICalculatePriceStrategy calculatePriceStrategy,
            IFindRidesWithLookingStatusStrategy findRidesWithLookingStatusStrategy,
            ICalculateRideDistanceStrategy calculateRideDistanceStrategy,
            ICheckWorkloadLevelStrategy checkWorkloadLevelStrategy,
            ICalculateLengthStrategy calculateLengthStrategy,
            IFindNearbyDriversStrategy findNearbyDriversStrategy)
        {
            _clientHubContext = clientHubContext ?? throw new ArgumentNullException(nameof(clientHubContext));
            _driverHubContext = driverHubContext ?? throw new ArgumentNullException(nameof(driverHubContext));
            _vuberDbContext = vuberDbContext;
            WorkloadLevel = WorkloadLevel.Normal;
            _calculateNewRatingStrategy = calculateNewRatingStrategy;
            _calculatePriceStrategy = calculatePriceStrategy;
            _findRidesWithLookingStatusStrategy = findRidesWithLookingStatusStrategy;
            _checkWorkloadLevelStrategy = checkWorkloadLevelStrategy;
            _calculateLengthStrategy = calculateLengthStrategy;
            _calculateRideDistanceStrategy = calculateRideDistanceStrategy;
            _findNearbyDriversStrategy = findNearbyDriversStrategy;
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
            return _calculateRideDistanceStrategy.Calculate(startLocation, targetLocations);
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
            WorkloadLevel = _checkWorkloadLevelStrategy.CheckWorkloadLevel(
                _vuberDbContext
                .Rides
                .Where(ride => ride.Status == RideStatus.Looking).ToList().Count);
        }

        private void WithdrawalForRide(Ride ride)
        {
            switch (ride.PaymentType)
            {
                case PaymentType.Cash:
                    _driverHubContext.Clients.User(ride.Driver.Id.ToString()).TakeCashPayment();
                    break;
                case PaymentType.PaymentCard:
                    var paymentCard = ride.Client.PaymentCard ?? throw new ArgumentNullException();
                    //а что здесь вообще? можно либо при создании карты класть на нее рандомное кол-во денег, но я даже не знаю...
                    break;
            }
        }

        private IEnumerable<Driver> NearbyDrivers(Coordinate coordinate, RideType rideType)
        {
            return _findNearbyDriversStrategy.FindNearbyDrivers(_vuberDbContext.Drivers, coordinate, rideType, _calculateLengthStrategy);
        }
    }
}