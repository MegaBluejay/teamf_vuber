using NetTopologySuite.Geometries;
using VuberCore.Dto;

namespace VuberClientClient.Controllers
{
    public class ClientNotificationController : IClientNotificationController
    {
        private bool _driverLocationUpdated = false;
        private Point _driverLocation;

        private bool _rideUpdated = false;
        private RideToClient _ride;

        public bool DriverLocation(out Point driverLocation)
        {
            driverLocation = _driverLocation;
            var updated = _driverLocationUpdated;
            _driverLocationUpdated = false;
            return updated;
        }

        public bool Ride(out RideToClient ride)
        {
            ride = _ride;
            var updated = _rideUpdated;
            _rideUpdated = false;
            return updated;
        }

        public void UpdateDriverLocation(Point driverLocation)
        {
            _driverLocationUpdated = true;
            _driverLocation = driverLocation;
        }

        public void UpdateRide(RideToClient ride)
        {
            _rideUpdated = true;
            _ride = ride;
        }
    }
}