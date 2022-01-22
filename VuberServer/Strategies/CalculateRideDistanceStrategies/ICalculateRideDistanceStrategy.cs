using NetTopologySuite.Geometries;

namespace VuberServer.Strategies.CalculateRideDistanceStrategies
{
    public interface ICalculateRideDistanceStrategy
    {
        decimal Calculate(LineString path);
    }
}