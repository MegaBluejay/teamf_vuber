using System.Collections.Generic;
using VuberCore.Dto;

namespace VuberDriverClient.Controllers
{
    public class DriverNotificationController : IDriverNotificationController
    {
        private readonly List<RideToDriver> _ridesRequested = new List<RideToDriver>();
        private bool _rideCancelled = false;
        private bool _takeCashPayment = false;

        public IEnumerable<RideToDriver> RidesRequested
        {
            get
            {
                var rides = _ridesRequested;
                _ridesRequested.Clear();
                return rides;
            }
        }

        public bool RideCancelled => _rideCancelled;

        public bool CashPayment => _takeCashPayment;

        public void AddRideRequested(RideToDriver rideToDriver) => _ridesRequested.Add(rideToDriver);

        public void CancelRide() => _rideCancelled = true;

        public void TakeCashPayment() => _takeCashPayment = true;
    }
}