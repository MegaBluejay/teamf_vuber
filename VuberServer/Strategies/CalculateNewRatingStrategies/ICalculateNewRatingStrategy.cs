using VuberCore.Entities;

namespace VuberServer.Strategies.CalculateNewRatingStrategies
{
    public interface ICalculateNewRatingStrategy
    {
        void CalculateNewRating(Rating driverRating, Mark lastRideMark);
    }
}