using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using VuberServer.Hubs;
using VuberCore.Entities;
using VuberCore.Data;

namespace VuberServer.Controllers
{
    public class VuberController : IVuberController
    {
        private readonly IHubContext<ClientHub> _clientHubContext;
        private readonly IHubContext<DriverHub> _driverHubContext;
        private readonly VuberDbContext _vuberDbContext;

        public VuberController(IHubContext<ClientHub> clientHubContext, IHubContext<DriverHub> driverHubContext)
        {
            _clientHubContext = clientHubContext ?? throw new ArgumentNullException(nameof(clientHubContext));
            _driverHubContext = driverHubContext ?? throw new ArgumentNullException(nameof(driverHubContext));
        }

        public void SetRating(Rating rating, Guid userId)
        {
            User user = _vuberDbContext.Users.FirstOrDefault(user => user.Id == userId);
            user.Rating = rating;
            _vuberDbContext.SaveChanges();
        }
    }
}