using VuberCore.Entities;

namespace VuberServer.Strategies.CheckWorkloadLevelStrategies
{
    public interface ICheckWorkloadLevelStrategy
    {
        public WorkloadLevel CheckWorkloadLevel(decimal ridesWithLookingStatusCount);
    }
}