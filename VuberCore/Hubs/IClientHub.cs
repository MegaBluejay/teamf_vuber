using System;
using System.Collections.Generic;
using VuberCore.Dto;
using VuberCore.Entities;

namespace VuberCore.Hubs
{
    public interface IClientHub : IVuberHub
    {
        void Register(NewClient newClient);
        void OrderRide(RideOrder rideOrder);
        void CancelOrder();
        void AddPaymentCard(string cardData);
        IEnumerable<RideToClient> SeeRides();
        Rating GetDriverRating(Guid driverGuid);
    }
}