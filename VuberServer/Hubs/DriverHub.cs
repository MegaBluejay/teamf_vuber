using System;
using System.Collections.Generic;
using System.Linq;
using Geolocation;
using VuberCore.Dto;
using VuberCore.Entities;
using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public class DriverHub : VuberHub<IDriverClient>
    {
        public DriverHub(IVuberController vuberController)
            : base(vuberController) { }


        public IEnumerable<RideToDriver> SeeRides()
        {
            return _vuberController.SeeRides(GetCurrentId()).Select(ride => new RideToDriver(ride));
        }

        public override Rating SeeRating(Guid rideId)
        {
            return _vuberController.SeeRating(rideId, ride => ride.Client);
        }

        public override void SetRating(Rating rating, Guid rideId)
        {
            _vuberController.SetRating(rating, rideId, ride => ride.Client);
        }

        public bool AcceptOrder(Guid rideId)
        {
            return _vuberController.DriverTakesRide(GetCurrentId(), rideId);
        }

        public void RejectOrder(Guid rideId)
        {
        }

        public void NotifyClientAboutArrival(Guid rideId)
        {
            _vuberController.DriverArrives(rideId);
        }

        public void SendCurrentLocation(Coordinate currentLocation)
        {
            _vuberController.UpdateDriverLocation(GetCurrentId(), currentLocation);
        }
    }
}