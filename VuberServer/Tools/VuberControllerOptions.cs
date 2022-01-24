using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using VuberCore.Entities;
using VuberServer.Clients;
using VuberServer.Controllers;
using VuberServer.Data;
using VuberServer.Hubs;
using VuberServer.Strategies.CalculateNewRatingStrategies;
using VuberServer.Strategies.CalculatePriceStrategies;
using VuberServer.Strategies.CalculateRideDistanceStrategies;
using VuberServer.Strategies.CheckWorkloadLevelStrategies;
using VuberServer.Strategies.FindNearbyDriversStrategies;
using VuberServer.Strategies.FindRidesWithLookingStatusStrategies;

namespace VuberServer.Tools
{
    public class VuberControllerOptions
    {
        public IHubContext<ClientHub, IClientClient> ClientHubContext { get; set; }
        public IHubContext<DriverHub, IDriverClient> DriverHubContext { get; set;  }
        public VuberDbContext DbContext { get; set; }
        public WorkloadLevel WorkloadLevel { get; set; }
        public ILogger<VuberController> Logger { get; set; }
        public ICalculateNewRatingStrategy CalculateNewRatingStrategy { get; set; }
        public ICalculatePriceStrategy CalculatePriceStrategy { get; set; }
        public ICheckWorkloadLevelStrategy CheckWorkloadLevelStrategy { get; set; }
        public IFindRidesWithLookingStatusStrategy FindRidesWithLookingStatusStrategy { get; set; }
        public ICalculateRideDistanceStrategy CalculateRideDistanceStrategy { get; set; }
        public ICalculateLengthStrategy CalculateLengthStrategy { get; set; }
        public IFindNearbyDriversStrategy FindNearbyDriversStrategy { get; set; }
        public IChronometer Chronometer { get; set; }

        public bool Validate()
        {
            if (ClientHubContext == null)
                return false;
            if (DriverHubContext == null)
                return false;
            if (DbContext == null)
                return false;
            if (Logger == null)
                return false;
            if (CalculateNewRatingStrategy == null)
                return false;
            if (CalculatePriceStrategy == null)
                return false;
            if (CheckWorkloadLevelStrategy == null)
                return false;
            if (FindRidesWithLookingStatusStrategy == null)
                return false;
            if (CalculateRideDistanceStrategy == null)
                return false;
            if (FindNearbyDriversStrategy == null)
                return false;
            if (Chronometer == null)
                return false;
            return true;
        }
    }
}