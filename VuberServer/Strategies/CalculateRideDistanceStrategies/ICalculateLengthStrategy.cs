using Geolocation;

namespace VuberServer.Strategies.CalculateRideDistanceStrategies
{
    public interface ICalculateLengthStrategy
    {
        decimal Calculate(Coordinate startLocation, Coordinate endLocation);
    }
}