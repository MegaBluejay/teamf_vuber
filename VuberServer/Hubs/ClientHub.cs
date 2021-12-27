using System;
using VuberCore.Entities;
using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public class ClientHub : VuberHub<IClientClient>
    {
        public ClientHub(IVuberController vuberController)
            : base(vuberController) { }

        public void OrderRide(Location startLocation, Location targetLocation, RideType rideType)
        {
        }

        public void AddPaymentCard(string cardData)
        {
        }

        public override void SeeRides()
        {
        }

        public override void SetRating(Rating rating, Guid rideId)
        {
        }
    }
}