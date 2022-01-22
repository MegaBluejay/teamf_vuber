using System;
using System.Collections.Generic;
using Geolocation;
using VuberCore.Dto;

namespace VuberCore.Hubs
{
    public interface IDriverHub : IVuberHub
    {
        IEnumerable<RideToDriver> SeeRides();
        bool AcceptOrder(Guid rideId);
        void RejectOrder(Guid rideId);
        void NotifyClientAboutArrival(Guid rideId);
        void SendCurrentLocation(Coordinate currentLocation);
    }
}