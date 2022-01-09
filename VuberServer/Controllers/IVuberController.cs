using System;
using System.Collections.Generic;
using VuberCore.Entities;
using Geolocation;

namespace VuberServer.Controllers
{
    public interface IVuberController
    {
        Ride CreateNewRide(
            Client client, 
            Coordinate startLocation, 
            ICollection<Coordinate> targetLocations,
            RideType rideType);
        
        List<Ride> SeeRides(User activeUser);
        
        void SetRating(Rating rating, Guid userId);
    }
}