using System;
using System.Collections.Generic;
using System.Linq;
using VuberCore.Dto;
using VuberCore.Entities;
using VuberCore.Hubs;
using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public class ClientHub : VuberHub<IClientClient>, IClientHub
    {
        public ClientHub(IVuberController vuberController)
            : base(vuberController) { }

        public void OrderRide(RideOrder rideOrder)
        {
            _vuberController.CreateNewRide(Guid.Empty, rideOrder.StartLocation, rideOrder.TargetLocations.ToList(),
                rideOrder.PaymentType, rideOrder.RideType);
        }

        public void CancelOrder() => throw new NotImplementedException();

        public void AddPaymentCard(string cardData) => _vuberController.AddPaymentCard(GetCurrentId(), cardData);

        public IEnumerable<RideToClient> SeeRides() => _vuberController.SeeRides(GetCurrentId()).Select(ride => new RideToClient(ride));

        public Rating GetDriverRating(Guid driverGuid) => throw new NotImplementedException();

        public override void SetRating(Mark mark, Guid rideId) => _vuberController.SetRating(mark, rideId, ride => ride.Driver);
    }
}