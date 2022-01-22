using System.Collections.Generic;
using NetTopologySuite.Geometries;
using VuberCore.Entities;
using VuberServer.Strategies.CalculateRideDistanceStrategies;

namespace VuberServer.Strategies.FindNearbyDriversStrategies
{
    public interface IFindNearbyDriversStrategy
    {
        IEnumerable<Driver> FindNearbyDrivers(
            IEnumerable<Driver> allDrivers,
            Point coordinate,
            RideType rideType,
            ICalculateLengthStrategy calculateLengthStrategy);
    }
}