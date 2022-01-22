using System.Collections.Generic;
using Geolocation;

namespace VuberServer.Strategies.CalculateRideDistanceStrategies
{
    public interface ICalculateRideDistanceStrategy
    {
        decimal Calculate(Coordinate startLocation, ICollection<Coordinate> targetLocations);
    }
}