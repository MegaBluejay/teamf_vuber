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
    public class ClientHub : VuberHub<IClientClient>
    {
        public ClientHub(IVuberController vuberController)
            : base(vuberController) { }

        public void OrderRide(RideOrder rideOrder)
        {
            _vuberController.CreateNewRide(Guid.Empty, rideOrder.StartLocation, rideOrder.TargetLocations.ToList(),
                rideOrder.PaymentType, rideOrder.RideType);
        }

        public void AddPaymentCard(string cardData)
        {
            _vuberController.AddPaymentCard(GetCurrentId(), cardData);
        }

        public IEnumerable<RideToClient> SeeRides()
        {
            return _vuberController.SeeRides(GetCurrentId()).Select(ride => new RideToClient(ride));
        }

        public override Rating SeeRating(Guid rideId)
        {
            return _vuberController.SeeRating(rideId, ride => ride.Client);
        }

        public override void SetRating(Rating rating, Guid rideId)
        {
            _vuberController.SetRating(rating, rideId, ride => ride.Driver);
        }
    }
}