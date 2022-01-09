using System.Collections.Generic;
using VuberCore.Entities;
using VuberCore.Data;

namespace VuberServer.Strategies.FindRidesWithLookingStatusStrategies
{
    public interface IFindRidesWithLookingStatusStrategy
    {
        List<Ride> FindRidesWithLookingStatus(VuberDbContext vuberDbContext);
    }
}