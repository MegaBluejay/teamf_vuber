using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using VuberServer.Hubs;
using VuberCore.Entities;
using VuberCore.Data;
using VuberServer.Strategies.CalculateNewRatingStrategies;
using VuberServer.Strategies.FindRidesWithLookingStatusStrategies;

namespace VuberServer.Controllers
{
    public class VuberController : IVuberController
    {
        private readonly IHubContext<ClientHub> _clientHubContext;
        private readonly IHubContext<DriverHub> _driverHubContext;
        private readonly VuberDbContext _vuberDbContext;
        private readonly ICalculateNewRatingStrategy _calculateNewRatingStrategy;

        public VuberController(
            IHubContext<ClientHub> clientHubContext,
            IHubContext<DriverHub> driverHubContext,
            VuberDbContext vuberDbContext,
            ICalculateNewRatingStrategy calculateNewRatingStrategy)
        {
            _clientHubContext = clientHubContext ?? throw new ArgumentNullException(nameof(clientHubContext));
            _driverHubContext = driverHubContext ?? throw new ArgumentNullException(nameof(driverHubContext));
            _vuberDbContext = vuberDbContext;
            _calculateNewRatingStrategy = calculateNewRatingStrategy;
        }

        public List<Ride> SeeRides(User activeUser)
        {
            return _vuberDbContext.Clients.FirstOrDefault(user => user.Id == activeUser.Id).Rides;
        }

        public void SetRating(Rating rating, Guid userId)
        {
            User user = _vuberDbContext.Users.FirstOrDefault(user => user.Id == userId);
            _calculateNewRatingStrategy.CalculateNewRating(user.Rating, rating);
            _vuberDbContext.SaveChanges();
        }
    }
}