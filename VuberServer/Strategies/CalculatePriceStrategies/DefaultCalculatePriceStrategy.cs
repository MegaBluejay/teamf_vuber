using VuberCore.Entities;

namespace VuberServer.Strategies.CalculatePriceStrategies
{
    public class DefaultCalculatePriceStrategy : ICalculatePriceStrategy
    {
        private readonly decimal _economyRideTypePriceMultiplier;
        private readonly decimal _comfortRideTypePriceMultiplier;
        private readonly decimal _businessRideTypePriceMultiplier;
        private readonly decimal _loadedLevelExtraCharge;
        public DefaultCalculatePriceStrategy(
            decimal economyRideTypePriceMultiplier, 
            decimal comfortRideTypePriceMultiplier,
            decimal businessRideTypePriceMultiplier,
            decimal loadedLevelExtraCharge)
        {
            _economyRideTypePriceMultiplier = economyRideTypePriceMultiplier;
            _comfortRideTypePriceMultiplier = comfortRideTypePriceMultiplier;
            _businessRideTypePriceMultiplier = businessRideTypePriceMultiplier;
            _loadedLevelExtraCharge = loadedLevelExtraCharge;
        }

        public decimal CalculatePrice(decimal rideLength, RideType rideType, WorkloadLevel workloadLevel)
        {
            decimal price = rideLength;
            switch (rideType)
            {
                case RideType.Economy:
                    price *= _economyRideTypePriceMultiplier;
                    break;
                case RideType.Comfort:
                    price *= _comfortRideTypePriceMultiplier;
                    break;
                case RideType.Business:
                    price *= _businessRideTypePriceMultiplier;
                    break;
            }
            
            if (workloadLevel == WorkloadLevel.Loaded)
            {
                price += _loadedLevelExtraCharge;
            }

            return price;
        }
    }
}
