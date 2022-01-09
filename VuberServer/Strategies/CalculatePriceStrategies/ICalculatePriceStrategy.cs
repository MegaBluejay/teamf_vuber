using VuberCore.Entities;

namespace VuberServer.Strategies.CalculatePriceStrategies
{
    public interface ICalculatePriceStrategy
    {
        decimal CalculatePrice(decimal rideLength, RideType rideType, WorkloadLevel workloadLevel);
    }
}
