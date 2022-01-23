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
        private IChronometer _chronometer;

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
            IFindNearbyDriversStrategy findNearbyDriversStrategy,
            IChronometer chronometer)
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
            _chronometer = chronometer;
        }

        public Ride CreateNewRide(
            string clientUsername,
            LineString path,
            PaymentType paymentType,
            RideType rideType)
        {
            var clientForRide = _vuberDbContext.Clients.FirstOrDefault(client => client.Username == clientUsername) ??
                                throw new ArgumentNullException();

            var ride = new Ride(
                clientForRide,
                paymentType,
                CalculatePrice(rideType, path),
                rideType,
                path,
                _chronometer);
            _vuberDbContext.Rides.Add(ride);
            _vuberDbContext.SaveChanges();
            var drivers = NearbyDrivers(path.StartPoint, rideType);
            _driverHubContext.Clients.Clients(drivers.Select(driver => driver.Username))
                .RideRequested(new RideToDriver(ride)).Start();
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
            (
                newDriver.MinRideLevel,
                newDriver.MaxRideLevel
            )
            {
                Username = newDriver.Username,
                Name = newDriver.Name,
            });
        }

        public bool DriverTakesRide(string driverUsername, Guid rideId)
        {
            var driverToTakeRide = _vuberDbContext.Drivers.FirstOrDefault(driver => driver.Username == driverUsername) ??
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
            _clientHubContext.Clients.User(rideToTake.Client.Username).UpdateRide(new RideToClient(rideToTake)).Start();
            _logger.LogInformation("Ride {0} has been taken by driver {1}", rideToTake.Id, driverToTakeRide.Id);
            return true;
        }

        public void DriverArrives(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                             throw new ArgumentNullException();
            ride.DriverArrives(_chronometer);
            _vuberDbContext.Rides.Update(ride);
            _vuberDbContext.SaveChanges();
            _clientHubContext.Clients.User(ride.Client.Username).UpdateRide(new RideToClient(ride)).Start();
            _logger.LogInformation("Driver arrived to client for ride {0}", ride.Id);
        }

        public void RideCompleted(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                       throw new ArgumentNullException();
            var coordinates = ride.Checkpoints.Select(checkpoint => checkpoint.Coordinate).ToList();
            WithdrawalForRide(ride, CalculatePrice(ride.RideType, ride.Path));
            ride.Finish(_chronometer);
            _vuberDbContext.Rides.Update(ride);
            _logger.LogInformation("Ride {0} completed", ride.Id);
        }

        public void PassCheckpoint(Guid rideId, int checkpointNumber)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                       throw new ArgumentNullException();
            ride.PassCheckpoint(checkpointNumber);
        }

        public void CancelRide(Guid rideId)
        {
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind => rideToFind.Id == rideId) ??
                             throw new ArgumentNullException();

            if (ride.Status == RideStatus.InProgress)
            {
                var coordinates = (from checkpoint in ride.Checkpoints where checkpoint.IsPassed select checkpoint.Coordinate).ToList();
                var money = CalculatePrice(ride.RideType, ride.Path);
                WithdrawalForRide(ride, money);
            }

            ride.Cancel(_chronometer);
            _vuberDbContext.Rides.Update(ride);
            _driverHubContext.Clients.User(ride.Driver.Username).RideCancelled().Start();
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

        public IReadOnlyList<Ride> SeeRides(string userUsername)
        {
            var user = _vuberDbContext.Clients.FirstOrDefault(userToFind => userToFind.Username == userUsername) ??
                        (User) (_vuberDbContext.Drivers.FirstOrDefault(userToFind => userToFind.Username == userUsername) ??
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

        public void AddPaymentCard(string clientUsername, string cardData)
        {
            var client = _vuberDbContext.Clients.FirstOrDefault(clientToFind => clientToFind.Username == clientUsername) ??
                         throw new ArgumentNullException();
            client.PaymentCard = new PaymentCard() {CardData = cardData};
            _vuberDbContext.SaveChanges();
            _logger.LogInformation("Client {0} added payment card", client.Id);
        }

        public void UpdateDriverLocation(string driverUsername, Point location)
        {
            var driver = _vuberDbContext.Drivers.FirstOrDefault(driverToFind => driverToFind.Username == driverUsername) ??
                         throw new ArgumentNullException();
            driver.UpdateLocation(location, _chronometer.TimeNow());
            _vuberDbContext.SaveChanges();
            var ride = _vuberDbContext.Rides.FirstOrDefault(rideToFind =>
                rideToFind.Driver.Id == driver.Id && rideToFind.Status == RideStatus.Waiting);
            if (ride != null)
            {
                _clientHubContext.Clients.User(ride.Client.Username).UpdateDriverLocation(location).Start();
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
                    _driverHubContext.Clients.User(ride.Driver.Username).TakeCashPayment().Start();
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