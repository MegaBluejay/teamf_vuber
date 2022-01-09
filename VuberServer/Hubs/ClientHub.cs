using System;
using System.Collections.Generic;
using Geolocation;
using VuberCore.Entities;
using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public class ClientHub : VuberHub<IClientClient>
    {
        public ClientHub(IVuberController vuberController)
            : base(vuberController) { }

        public void OrderRide(Coordinate startLocation, ICollection<Coordinate> targetLocations, RideType rideType)
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