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
        }

        public VuberControllerOptionsBuilder Reset()
        {
            _options = new VuberControllerOptions();
            return this;
        }

        public VuberControllerOptionsBuilder UseClientHubContext(IHubContext<ClientHub, IClientClient> clientHubContext)
        {
            _options.ClientHubContext = clientHubContext;
            return this;
        }

        public VuberControllerOptionsBuilder UseDriverHubContext(IHubContext<DriverHub, IDriverClient> driverHubContext)
        {
            _options.DriverHubContext = driverHubContext;
            return this;
        }

        public VuberControllerOptionsBuilder UseDbContext(VuberDbContext dbContext)
        {
            _options.DbContext = dbContext;
            return this;
        }

        public VuberControllerOptionsBuilder WorkloadLevel(WorkloadLevel workloadLevel)
        {
            _options.WorkloadLevel = workloadLevel;
            return this;
        }

        public VuberControllerOptionsBuilder UseLogger(ILogger<VuberController> logger)
        {
            _options.Logger = logger;
            return this;
        }

        public VuberControllerOptionsBuilder CalculateNewRatingStrategy(ICalculateNewRatingStrategy strategy)
        {
            _options.CalculateNewRatingStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder CalculatePriceStrategy(ICalculatePriceStrategy strategy)
        {
            _options.CalculatePriceStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder CheckWorkloadLevelStrategy(ICheckWorkloadLevelStrategy strategy)
        {
            _options.CheckWorkloadLevelStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder FindRidesWithLookingStatusStrategy(
            IFindRidesWithLookingStatusStrategy strategy)
        {
            _options.FindRidesWithLookingStatusStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder CalculateRideDistanceStrategy(ICalculateRideDistanceStrategy strategy)
        {
            _options.CalculateRideDistanceStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder CalculateLengthStrategy(ICalculateLengthStrategy strategy)
        {
            _options.CalculateLengthStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder FindNearbyDriversStrategy(IFindNearbyDriversStrategy strategy)
        {
            _options.FindNearbyDriversStrategy = strategy;
            return this;
        }

        public VuberControllerOptionsBuilder Chronometer(IChronometer chronometer)
        {
            _options.Chronometer = chronometer;
            return this;
        }
    }
}