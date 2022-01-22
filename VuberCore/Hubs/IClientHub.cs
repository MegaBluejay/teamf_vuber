using System.Collections.Generic;
using VuberCore.Dto;

namespace VuberCore.Hubs
{
    public interface IClientHub : IVuberHub
    {
        void OrderRide(RideOrder rideOrder);
        void AddPaymentCard(string cardData);
        IEnumerable<RideToClient> SeeRides();
    }
}