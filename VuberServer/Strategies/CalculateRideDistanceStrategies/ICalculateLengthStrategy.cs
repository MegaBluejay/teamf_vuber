using NetTopologySuite.Geometries;

namespace VuberServer.Strategies.CalculateRideDistanceStrategies
{
    public interface ICalculateLengthStrategy
    {
        decimal Calculate(Point startLocation, Point endLocation);
    }
}