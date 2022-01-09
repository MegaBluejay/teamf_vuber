using System;
using System.Collections.Generic;
using VuberCore.Entities;
using Geolocation;

namespace VuberServer.Controllers
{
    public interface IVuberController
    {
        Ride CreateNewRide(
            Guid clientId, 
            Coordinate startLocation, 
            ICollection<Coordinate> targetLocations,
            RideType rideType);

        void DriverTakesRide(Guid driverId, Guid rideId);
        
        List<Ride> SeeRides(User activeUser);
        
        void SetRating(Rating rating, Guid userId);
    }
}