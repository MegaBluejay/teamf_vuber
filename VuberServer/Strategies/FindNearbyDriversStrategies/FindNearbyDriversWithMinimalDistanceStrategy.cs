using Geolocation;
using System.Collections.Generic;
using System.Linq;
using VuberCore.Entities;
using VuberServer.Strategies.CalculateRideDistanceStrategies;

namespace VuberServer.Strategies.FindNearbyDriversStrategies
{
    public class FindNearbyDriversWithMinimalDistanceStrategy : IFindNearbyDriversStrategy
    {
        private decimal _minimalDistance;

        public FindNearbyDriversWithMinimalDistanceStrategy(decimal minimalDistance)
        {
            _minimalDistance = minimalDistance;
        }

        public IEnumerable<Driver> FindNearbyDrivers(
            IEnumerable<Driver> allDrivers,
            Coordinate coordinate,
            RideType rideType,
            ICalculateLengthStrategy calculateLengthStrategy)
        {
            return allDrivers.Where(driver =>
                calculateLengthStrategy.Calculate(driver.LastKnownLocation, coordinate) < _minimalDistance &&
                driver.MinRideLevel <= rideType && rideType <= driver.MaxRideLevel);
        }
    }
}