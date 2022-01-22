using VuberCore.Entities;

namespace VuberServer.Strategies.CalculateNewRatingStrategies
{
    public class ArithmeticalMeanCalculateNewRatingStrategy : ICalculateNewRatingStrategy
    {
        public void CalculateNewRating(Rating driverRating, Mark lastRideMark)
        {
            driverRating.Value = new Mark((driverRating.Value.Value * driverRating.RidesNumber + lastRideMark.Value) / (driverRating.RidesNumber + 1));
            ++driverRating.RidesNumber;
        }
    }
}