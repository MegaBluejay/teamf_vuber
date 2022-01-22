using NetTopologySuite.Geometries;
using VuberCore.Dto;

namespace VuberClientClient.Controllers
{
    public interface IClientNotificationController
    {
        bool DriverLocation(out Point driverLocation);
        bool Ride(out RideToClient ride);

        void UpdateDriverLocation(Point driverLocation);
        void UpdateRide(RideToClient ride);
    }
}