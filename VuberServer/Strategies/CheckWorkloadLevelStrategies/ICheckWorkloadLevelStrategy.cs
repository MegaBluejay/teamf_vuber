using VuberCore.Entities;

namespace VuberServer.Strategies.CheckWorkloadLevelStrategies
{
    public interface ICheckWorkloadLevelStrategy
    {
        WorkloadLevel CheckWorkloadLevel(decimal ridesWithLookingStatusCount);
    }
}