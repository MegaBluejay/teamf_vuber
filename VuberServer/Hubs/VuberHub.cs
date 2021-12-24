using System;
using Microsoft.AspNetCore.SignalR;
using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public abstract class VuberHub<TClient> : Hub<TClient>
        where TClient : class, IVuberClient
    {
        protected readonly IVuberController _vuberController;

        public VuberHub(IVuberController vuberController)
        {
            _vuberController = vuberController ?? throw new ArgumentNullException(nameof(vuberController));
        }
    }
}