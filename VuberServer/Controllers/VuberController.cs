using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using VuberCore.Entities;
using VuberServer.Hubs;
using VuberCore.Dto;
using VuberServer.Clients;
using VuberServer.Data;
using VuberServer.Strategies.CalculateNewRatingStrategies;
using VuberServer.Strategies.CalculatePriceStrategies;
using VuberServer.Strategies.CheckWorkloadLevelStrategies;
using VuberServer.Strategies.CalculateRideDistanceStrategies;
using VuberServer.Strategies.FindRidesWithLookingStatusStrategies;
using VuberServer.Strategies.FindNearbyDriversStrategies;


namespace VuberServer.Controllers
{
    public class VuberController : IVuberController
    {
        private readonly IHubContext<ClientHub, IClientClient> _clientHubContext;
        private readonly IHubContext<DriverHub, IDriverClient> _driverHubContext;
        private readonly VuberDbContext _vuberDbContext;
        private WorkloadLevel WorkloadLevel;
        private ILogger<VuberController> _logger;
        private ICalculateNewRatingStrategy _calculateNewRatingStrategy;
        private ICalculatePriceStrategy _calculatePriceStrategy;
        private ICheckWorkloadLevelStrategy _checkWorkloadLevelStrategy;
        private IFindRidesWithLookingStatusStrategy _findRidesWithLookingStatusStrategy;
        private ICalculateRideDistanceStrategy _calculateRideDistanceStrategy;
        private ICalculateLengthStrategy _calculateLengthStrategy;
        private IFindNearbyDriversStrategy _findNearbyDriversStrategy;

        public VuberController(
            IHubContext<ClientHub, IClientClient> clientHubContext,
            IHubContext<DriverHub, IDriverClient> driverHubContext,
            VuberDbContext vuberDbContext,
            ILogger<VuberController> logger,
            ICalculateNewRatingStrategy calculateNewRatingStrategy,
            ICalculatePriceStrategy calculatePriceStrategy,
            IFindRidesWithLookingStatusStrategy findRidesWithLookingStatusStrategy,
            ICalculateRideDistanceStrategy calculateRideDistanceStrategy,
            ICheckWorkloadLevelStrategy checkWorkloadLevelStrategy,
            ICalculateLengthStrategy calculateLengthStrategy,
            IFindNearbyDriversStrategy findNearbyDriversStrategy)
        {
            _clientHubContext = clientHubContext ?? throw new ArgumentNullException(nameof(clientHubContext));
            _driverHubContext = driverHubContext ?? throw new ArgumentNullException(nameof(driverHubContext));
            _vuberDbContext = vuberDbContext;
            WorkloadLevel = WorkloadLevel.Normal;
            _logger = logger;
            _calculateNewRatingStrategy = calculateNewRatingStrategy;
            _calculatePriceStrategy = calculatePriceStrategy;
            _findRidesWithLookingStatusStrategy = findRidesWithLookingStatusStrategy;
            _checkWorkloadLevelStrategy = checkWorkloadLevelStrategy;
            _calculateLengthStrategy = calculateLengthStrategy;
            _calculateRideDistanceStrategy = calculateRideDistanceStrategy;
            _findNearbyDriversStrategy = findNearbyDriversStrategy;
        }

        public Ride CreateNewRide(
            Guid clientId,
            LineString path,
            PaymentType paymentType,
            RideType rideType)
        {
            var clientForRide = _vuberDbContext.Clients.FirstOrDefault(client => client.Id == clientId) ??
                                throw new ArgumentNullException();

            var checkpoints = path.Coordinates.Skip(1).Select(coordinate => new Checkpoint() {Coordinate = new Point(coordinate), IsPassed = false,}).ToList();

            var ride = new Ride()
            {
                Client = clientForRide,
                Cost = CalculatePrice(rideType, path),
                PaymentType = paymentType,
                RideType = rideType,
                Status = RideStatus.Looking,
                Path = path,
                Checkpoints = checkpoints,
                Created = DateTime.UtcNow,
            };
            _vuberDbContext.Rides.Add(ride);
            _vuberDbContext.SaveChanges();
            var drivers = NearbyDrivers(path.StartPoint, rideType);
            _driverHubContext.Clients.Clients(drivers.Select(driver => driver.Id.ToString()))
                .RideRequested(new RideToDriver(ride));
            _logger.LogInformation("Ride {0} created", ride.Id);
            return ride;
        }
        public void RegisterClient(NewClient newClient)
        {
            _vuberDbContext.Clients.Add(new Client
            {
                Username = newClient.Username,
                Name = newClient.Name,
                PaymentCard = newClient.PaymentCard
            });
            _vuberDbContext.SaveChanges();
        }

        public void RegisterDriver(NewDriver newDriver)
        {
            _vuberDbContext.Drivers.Add(new Driver
            {
                Username = newDriver.Username,
                Name = newDriver.Name,
                MinRideLevel = newDriver.MinRideLevel,
                MaxRideLevel = newDriver.MaxRideLevel
            });
        }

        public bool DriverTakesRide(Guid driverId, Guid rideId)
        {
            var driverToTakeRide = _vuberDbContext.Drivers.FirstOrDefault(driver => driver.Id == driverId) ??
                                   throw new ArgumentNullException();
            var rideToTake = _vuberDbContext.Rides.FirstOrDefault(ride => ride.Id == rideId) ??
                             throw new ArgumentNullException();
            if (rideToTake.Status != RideStatus.Looking)
            {
                return false;
            }
            rideToTake.Driver = driverToTakeRide;
            rideToTake.Status = RideStatus.Waiting;
            rideToTake.Found = DateTime.UtcNow;
            _vuberDbContext.Rides.Update(rideToTake);
            _vuberDbContext.SaveChanges();
            _clientHubContext.Clients.User(rideToTake.Client.Id.ToString()).UpdateRide(new RideToClient(rideToTake));
            _logger.LogInformation("Ride {0} has been taken by driver {1}", rideToTake.Id, driverToTakeRide.Id);
            return true;
        }

        public void DriverArrives(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                             throw new ArgumentNullException();
            ride.Status = RideStatus.InProgress;
            ride.Started = DateTime.UtcNow;
            _vuberDbContext.Rides.Update(ride);
            _vuberDbContext.SaveChanges();
            _clientHubContext.Clients.User(ride.Client.Id.ToString()).UpdateRide(new RideToClient(ride));
            _logger.LogInformation("Driver arrived to client for ride {0}", ride.Id);
        }

        public void RideCompleted(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                       throw new ArgumentNullException();
            var coordinates = ride.Checkpoints.Select(checkpoint => checkpoint.Coordinate).ToList();
            WithdrawalForRide(ride, CalculatePrice(ride.RideType, ride.Path));
            ride.Status = RideStatus.Complete;
            ride.Finished = DateTime.UtcNow;
            _vuberDbContext.Rides.Update(ride);
            _logger.LogInformation("Ride {0} completed", ride.Id);
        }

        public void PassCheckpoint(Guid rideId, int checkpointNumber)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                       throw new ArgumentNullException();
            ride.Checkpoints[checkpointNumber].IsPassed = true;
        }

        public void CancelRide(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                             throw new ArgumentNullException();
            switch (ride.Status)
            {
                case RideStatus.Looking:
                    ride.Status = RideStatus.Cancelled;
                    break;
                case RideStatus.Waiting:
                    ride.Status = RideStatus.Cancelled;
                    break;
                case RideStatus.InProgress:
                    ride.Status = RideStatus.Cancelled;
                    var coordinates = (from checkpoint in ride.Checkpoints where checkpoint.IsPassed select checkpoint.Coordinate).ToList();
                    var money = CalculatePrice(ride.RideType, ride.Path);
                    WithdrawalForRide(ride, money);
                    break;
            }
            ride.Finished = DateTime.UtcNow;
            _vuberDbContext.Rides.Update(ride);
            _driverHubContext.Clients.User(ride.Driver.Id.ToString()).RideCancelled();
            _logger.LogInformation("Ride {0} canceled", ride.Id);
        }

        private decimal CalculateRideLength(LineString path)
        {
            return _calculateRideDistanceStrategy.Calculate(path);
        }

        private decimal CalculatePrice(RideType rideType, LineString path)
        {
            var rideLength = CalculateRideLength(path);
            CheckWorkloadLevel();
            return _calculatePriceStrategy.CalculatePrice(rideLength, rideType, WorkloadLevel);
        }

        public List<Ride> SeeRides(Guid userId)
        {
            var user = _vuberDbContext.Clients.FirstOrDefault(userToFind => userToFind.Id == userId) ??
                        (User) (_vuberDbContext.Drivers.FirstOrDefault(userToFind => userToFind.Id == userId) ??
                                throw new ArgumentNullException());
            var rides = user.Rides;
            _logger.LogInformation("Rides of user {0} returned", user.Id);
            return rides;
        }

        public void SetRating(Mark mark, Guid rideId, Func<Ride, User> userGetter)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(ride => ride.Id == rideId);
            var user = userGetter.Invoke(ride);
            _calculateNewRatingStrategy.CalculateNewRating(user.Rating, mark);
            _vuberDbContext.SaveChanges();
            _logger.LogInformation("Rating of user {0} set to {1}", user.Id, mark.Value);
        }

        public void AddPaymentCard(Guid clientId, string cardData)
        {
            var client = _vuberDbContext.Clients.FirstOrDefault(clientToFind => clientToFind.Id == clientId) ??
                         throw new ArgumentNullException();
            client.PaymentCard = new PaymentCard() {CardData = cardData};
            _vuberDbContext.SaveChanges();
            _logger.LogInformation("Client {0} added payment card", client.Id);
        }

        public void UpdateDriverLocation(Guid driverId, Point location)
        {
            var driver = _vuberDbContext.Drivers.FirstOrDefault(driverToFind => driverToFind.Id == driverId) ??
                         throw new ArgumentNullException();
            driver.LastKnownLocation = location;
            driver.LocationUpdatedAt = DateTime.UtcNow;
            _vuberDbContext.SaveChanges();
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind =>
                rideToFind.Driver.Id == driverId && rideToFind.Status == RideStatus.Waiting);
            if (ride != null)
            {
                _clientHubContext.Clients.User(ride.Client.Id.ToString()).UpdateDriverLocation(location);
            }
            _logger.LogInformation("Driver {0} location updated", driver.Id);
        }

        private List<Ride> FindRidesWithLookingStatus()
        {
            return _findRidesWithLookingStatusStrategy.FindRidesWithLookingStatus(_vuberDbContext);
        }

        private void CheckWorkloadLevel()
        {
            WorkloadLevel = _checkWorkloadLevelStrategy.CheckWorkloadLevel(
                _vuberDbContext
                .Rides
                .Where(ride => ride.Status == RideStatus.Looking).ToList().Count);
        }

        private void WithdrawalForRide(Ride ride, decimal money)
        {
            switch (ride.PaymentType)
            {
                case PaymentType.Cash:
                    _driverHubContext.Clients.User(ride.Driver.Id.ToString()).TakeCashPayment();
                    break;
                case PaymentType.PaymentCard:
                    var paymentCard = ride.Client.PaymentCard ?? throw new ArgumentNullException();
                    paymentCard.Money -= money;
                    break;
            }
        }

        private IEnumerable<Driver> NearbyDrivers(Point coordinate, RideType rideType)
        {
            return _findNearbyDriversStrategy.FindNearbyDrivers(_vuberDbContext.Drivers, coordinate, rideType, _calculateLengthStrategy);
        }
    }
}