using System.Collections.Generic;
using System.Linq;
using VuberCore.Entities;
using VuberServer.Data;

namespace VuberServer.Strategies.FindRidesWithLookingStatusStrategies
{
    public class DefaultFindRidesWithLookingStatusStrategy : IFindRidesWithLookingStatusStrategy
    {
        public List<Ride> FindRidesWithLookingStatus(VuberDbContext vuberDbContext)
        {
            return vuberDbContext.Rides.Where(ride => ride.Status == RideStatus.Looking).ToList();
        }
    }
}
