using Geolocation;
using System.Collections.Generic;
using VuberCore.Entities;
using VuberServer.Strategies.CalculateRideDistanceStrategies;

namespace VuberServer.Strategies.FindNearbyDriversStrategies
{
    public interface IFindNearbyDriversStrategy
    {
        IEnumerable<Driver> FindNearbyDrivers(
            IEnumerable<Driver> allDrivers,
            Coordinate coordinate,
            RideType rideType,
            ICalculateLengthStrategy calculateLengthStrategy);
    }
}