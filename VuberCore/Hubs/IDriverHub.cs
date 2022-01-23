using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using VuberCore.Dto;

namespace VuberCore.Hubs
{
    public interface IDriverHub : IVuberHub
    {
        void Register(NewDriver newDriver);
        IEnumerable<RideToDriver> SeeRides();
        bool AcceptOrder(Guid rideId);
        void RejectOrder(Guid rideId);
        void NotifyClientAboutArrival(Guid rideId);
        void SendCurrentLocation(Point currentLocation);
    }
}