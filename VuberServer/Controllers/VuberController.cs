using System;
using System.Collections.Generic;
using System.Linq;
using Geolocation;
using Microsoft.AspNetCore.SignalR;
using VuberCore.Entities;
using VuberServer.Hubs;
using VuberCore.Data;

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
        
        public VuberController(IHubContext<ClientHub> clientHubContext, IHubContext<DriverHub> driverHubContext, VuberDbContext vuberDbContext)
        {
            _clientHubContext = clientHubContext ?? throw new ArgumentNullException(nameof(clientHubContext));
            _driverHubContext = driverHubContext ?? throw new ArgumentNullException(nameof(driverHubContext));
            _vuberDbContext = vuberDbContext;
            WorkloadLevel = WorkloadLevel.Normal;
        }

        public List<Ride> SeeRides(User activeUser)
        {
            return _vuberDbContext.Clients.FirstOrDefault(user => user.Id == activeUser.Id).Rides;
        }

        public void SetRating(Rating rating, Guid userId)
        {
            User userToSetRating = _vuberDbContext.Users.FirstOrDefault(user => user.Id == userId) ?? throw new ArgumentNullException();
            userToSetRating.Rating = rating;
            _vuberDbContext.SaveChanges();
        }

        public Ride CreateNewRide(Client client, Coordinate startLocation, ICollection<Coordinate> targetLocations, RideType rideType)
        {
            return new Ride()
            {
                Client = client, 
                Cost = CalculatePrice(rideType, startLocation, targetLocations), 
                RideType = rideType, 
                Status = RideStatus.Looking, 
                StartLocation = startLocation, 
                TargetLocations = targetLocations, 
                Created = DateTime.UtcNow,
            };
        }

        private decimal CalculatePrice(RideType rideType, Coordinate startLocation, ICollection<Coordinate> targetLocations)
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

            var price = Convert.ToDecimal(length);
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
    }
}