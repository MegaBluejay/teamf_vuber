using System.Collections.Generic;
using VuberCore.Dto;

namespace VuberDriverClient.Controllers
{
    public interface IDriverNotificationController
    {
        IEnumerable<RideToDriver> RidesRequested { get; }
        bool RideCancelled { get; }
        void AddRideRequested(RideToDriver rideToDriver);
        void CancelRide();
    }
}