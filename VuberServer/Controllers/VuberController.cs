using System;
using System.Collections.Generic;
using System.Linq;
using Geolocation;
using Microsoft.AspNetCore.SignalR;
using VuberCore.Entities;
using VuberServer.Hubs;
using VuberCore.Data;
using VuberServer.Strategies.CalculateNewRatingStrategies;
using VuberServer.Strategies.FindRidesWithLookingStatusStrategies;

namespace VuberServer.Controllers
{
    public class VuberController : IVuberController
    {
        private readonly IHubContext<ClientHub> _clientHubContext;
        private readonly IHubContext<DriverHub> _driverHubContext;
        private readonly VuberDbContext _vuberDbContext;
        private const decimal EconomyRideTypePriceMultiplier = 50;
        private const decimal ComfortRideTypePriceMultiplier = 100;
        private const decimal BusinessRideTypePriceMultiplier = 200;
        private const decimal LoadedLevelExtraCharge = 100;
        private WorkloadLevel WorkloadLevel;
        private readonly ICalculateNewRatingStrategy _calculateNewRatingStrategy;
        private readonly IFindRidesWithLookingStatusStrategy _findRidesWithLookingStatusStrategy;

        public VuberController(
            IHubContext<ClientHub> clientHubContext,
            IHubContext<DriverHub> driverHubContext,
            VuberDbContext vuberDbContext,
            ICalculateNewRatingStrategy calculateNewRatingStrategy,
            IFindRidesWithLookingStatusStrategy findRidesWithLookingStatusStrategy)
        {
            _clientHubContext = clientHubContext ?? throw new ArgumentNullException(nameof(clientHubContext));
            _driverHubContext = driverHubContext ?? throw new ArgumentNullException(nameof(driverHubContext));
            _vuberDbContext = vuberDbContext;
            WorkloadLevel = WorkloadLevel.Normal;
            _calculateNewRatingStrategy = calculateNewRatingStrategy;
            _findRidesWithLookingStatusStrategy = findRidesWithLookingStatusStrategy;
        }

        public Ride CreateNewRide(Guid clientId, Coordinate startLocation, ICollection<Coordinate> targetLocations, RideType rideType)
        {
            var clientForRide = _vuberDbContext.Clients.FirstOrDefault(client => client.Id == clientId) ??
                                throw new ArgumentNullException();
            var ride = new Ride()
            {
                Client = clientForRide, 
                Cost = CalculatePrice(rideType, startLocation, targetLocations), 
                RideType = rideType, 
                Status = RideStatus.Looking, 
                StartLocation = startLocation, 
                TargetLocations = targetLocations, 
                Created = DateTime.UtcNow,
            };
            _vuberDbContext.Rides.Add(ride);
            return ride;
        }

        public void DriverTakesRide(Guid driverId, Guid rideId)
        {
            var driverToTakeRide = _vuberDbContext.Drivers.FirstOrDefault(driver => driver.Id == driverId) ?? 
                                   throw new ArgumentNullException();
            var rideToTake = _vuberDbContext.Rides.FirstOrDefault(ride => ride.Id == rideId) ?? 
                             throw new ArgumentNullException();
            rideToTake.Driver = driverToTakeRide;
            rideToTake.Status = RideStatus.Waiting;
            _vuberDbContext.Rides.Update(rideToTake);
        }

        public void DriverArrives(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ?? 
                             throw new ArgumentNullException();
            ride.Status = RideStatus.InProgress;
            _vuberDbContext.Rides.Update(ride);
        }

        public void RideCompleted(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ?? 
                       throw new ArgumentNullException();
            ride.Status = RideStatus.Complete;
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
                    //снятие денег с клиента за часть поездки
                    break;
                default:
                    throw new Exception("Ride cannot be cancelled");
            }
            _vuberDbContext.Rides.Update(ride);
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
            var price = CalculateRideLength(startLocation, targetLocations);
            switch (rideType)
            {
                case RideType.Economy:
                    price = price * EconomyRideTypePriceMultiplier + LoadedLevelExtraCharge;
                    break;
                case RideType.Comfort:
                    price = price * ComfortRideTypePriceMultiplier + LoadedLevelExtraCharge;
                    break;
                case RideType.Business:
                    price = price * BusinessRideTypePriceMultiplier + LoadedLevelExtraCharge;
                    break;
            }

            return price;
        }

        public List<Ride> SeeRides(User activeUser)
        {
            return _vuberDbContext.Clients.FirstOrDefault(user => user.Id == activeUser.Id).Rides;
        }
        
        public void SetRating(Rating rating, Guid userId)
        {
            User userToSetRating = _vuberDbContext.Users.FirstOrDefault(user => user.Id == userId)  ?? 
                                   throw new ArgumentNullException();
            _calculateNewRatingStrategy.CalculateNewRating(userToSetRating.Rating, rating);
            _vuberDbContext.SaveChanges();
        }

        public List<Ride> FindRidesWithLookingStatus()
        {
            return _findRidesWithLookingStatusStrategy.FindRidesWithLookingStatus(_vuberDbContext);
        }
    }
}