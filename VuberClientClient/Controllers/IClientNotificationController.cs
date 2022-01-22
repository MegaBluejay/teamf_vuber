using Geolocation;
using VuberCore.Dto;

namespace VuberClientClient.Controllers
{
    public interface IClientNotificationController
    {
        bool DriverLocation(out Coordinate driverLocation);
        bool Ride(out RideToClient ride);

        void UpdateDriverLocation(Coordinate driverLocation);
        void UpdateRide(RideToClient ride);
    }
}