using VuberCore.Entities;

namespace VuberServer.Strategies.CalculateNewRatingStrategies
{
    public class ArithmeticalMeanCalculateNewRatingStrategy : ICalculateNewRatingStrategy
    {
        public void CalculateNewRating(Rating driverRating, Rating lastRideRating)
        {
            driverRating.Value = (driverRating.Value * driverRating.RidesNumber + lastRideRating.Value) / (driverRating.RidesNumber + 1);
            ++driverRating.RidesNumber;
        }
    }
}