using System;
using Microsoft.AspNetCore.SignalR;
using VuberCore.Entities;
using VuberCore.Hubs;
using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public abstract class VuberHub<TClient> : Hub<TClient>, IVuberHub
        where TClient : class, IVuberClient
    {
        protected readonly IVuberController _vuberController;

        protected VuberHub(IVuberController vuberController) => _vuberController = vuberController ?? throw new ArgumentNullException(nameof(vuberController));

        public abstract void SetRating(Rating rating, Guid rideId);

        protected Guid GetCurrentId() => Guid.Parse(Context.User.Identity.Name);
    }
}