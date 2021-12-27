using System;
using VuberCore.Entities;
using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public class DriverHub : VuberHub<IDriverClient>
    {
        public DriverHub(IVuberController vuberController)
            : base(vuberController) { }


        public override void SeeRides()
        {
        }

        public override void SetRating(Rating rating, Guid rideId)
        {
        }

        public void AcceptOrder()
        {
        }

        public void RejectOrder()
        {
        }

        public void NotifyClientAboutArrival()
        {
        }

        public void SendCurrentLocation(Location currentLocation)
        {
        }
    }
}