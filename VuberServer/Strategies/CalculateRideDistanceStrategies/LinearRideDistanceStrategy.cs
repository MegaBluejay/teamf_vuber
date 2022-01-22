using NetTopologySuite.Geometries;

namespace VuberServer.Strategies.CalculateRideDistanceStrategies
{
    public class LinearRideDistanceStrategy : ICalculateRideDistanceStrategy
    {
        public decimal Calculate(LineString path)
        {
            return (decimal) path.Length;
        }
    }
}