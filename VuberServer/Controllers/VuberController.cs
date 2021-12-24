using System;
using Microsoft.AspNetCore.SignalR;
using VuberServer.Hubs;

namespace VuberServer.Controllers
{
    public class VuberController : IVuberController
    {
        private readonly IHubContext<ClientHub> _clientHubContext;
        private readonly IHubContext<DriverHub> _driverHubContext;

        public VuberController(IHubContext<ClientHub> clientHubContext, IHubContext<DriverHub> driverHubContext)
        {
            _clientHubContext = clientHubContext ?? throw new ArgumentNullException(nameof(clientHubContext));
            _driverHubContext = driverHubContext ?? throw new ArgumentNullException(nameof(driverHubContext));
        }
    }
}