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
    public class VuberControllerOptionsBuilder
    {
        private VuberControllerOptions _options;

        public VuberControllerOptionsBuilder()
        {
            Reset();
        }

        public VuberControllerOptions Options
        {
            get
            {
                var options = _options;
                Reset();
                return options;
            }
            private set => _options = value;
        }

        public VuberControllerOptionsBuilder Reset()
        {
            Options = new VuberControllerOptions();
            return this;
        }

        public VuberControllerOptionsBuilder UseClientHubContext(IHubContext<ClientHub, IClientClient> clientHubContext)
        {
            Options.ClientHubContext = clientHubContext;
            return this;
        }

        public VuberControllerOptionsBuilder UseDriverHubContext(IHubContext<DriverHub, IDriverClient> driverHubContext)
        {
            Options.DriverHubContext = driverHubContext;
            return this;
        }

        public VuberControllerOptionsBuilder UseDbContext(VuberDbContext dbContext)
        {
            Options.DbContext = dbContext;
            return this;
        }

        public VuberControllerOptionsBuilder WorkloadLevel(WorkloadLevel workloadLevel)
        {
            Options.WorkloadLevel = workloadLevel;
            return this;
        }

        public VuberControllerOptionsBuilder UseLogger(ILogger<VuberController> logger)
        {
            Options.Logger = logger;
            return this;
        }

        public VuberControllerOptionsBuilder CalculateNewRatingStrategy(ICalculateNewRatingStrategy strategy)
        {
            Options.CalculateNewRatingStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder CalculatePriceStrategy(ICalculatePriceStrategy strategy)
        {
            Options.CalculatePriceStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder CheckWorkloadLevelStrategy(ICheckWorkloadLevelStrategy strategy)
        {
            Options.CheckWorkloadLevelStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder FindRidesWithLookingStatusStrategy(
            IFindRidesWithLookingStatusStrategy strategy)
        {
            Options.FindRidesWithLookingStatusStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder CalculateRideDistanceStrategy(ICalculateRideDistanceStrategy strategy)
        {
            Options.CalculateRideDistanceStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder CalculateLengthStrategy(ICalculateLengthStrategy strategy)
        {
            Options.CalculateLengthStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder FindNearbyDriversStrategy(IFindNearbyDriversStrategy strategy)
        {
            Options.FindNearbyDriversStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder Chronometer(IChronometer chronometer)
        {
            Options.Chronometer = chronometer;
            return this;
        }
    }
}