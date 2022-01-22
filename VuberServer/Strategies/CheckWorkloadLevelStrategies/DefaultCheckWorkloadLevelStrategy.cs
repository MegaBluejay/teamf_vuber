using VuberCore.Entities;

namespace VuberServer.Strategies.CheckWorkloadLevelStrategies
{
    public class DefaultCheckWorkloadLevelStrategy : ICheckWorkloadLevelStrategy
    {
        private readonly decimal _maxLookingRidesForNormalWorkloadLevel;

        public DefaultCheckWorkloadLevelStrategy(decimal maxLookingRidesForNormalWorkloadLevel)
        {
            _maxLookingRidesForNormalWorkloadLevel = maxLookingRidesForNormalWorkloadLevel;
        }

        public WorkloadLevel CheckWorkloadLevel(decimal ridesWithLookingStatusCount)
        {
            var resultWorkloadLevel = WorkloadLevel.Normal;
            if (ridesWithLookingStatusCount > _maxLookingRidesForNormalWorkloadLevel)
            {
                resultWorkloadLevel = WorkloadLevel.Loaded;
            }

            return resultWorkloadLevel;
        }
    }
}