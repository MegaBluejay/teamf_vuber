using System;
using Microsoft.AspNetCore.SignalR;
using VuberCore.Entities;
using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public abstract class VuberHub<TClient> : Hub<TClient>
        where TClient : class, IVuberClient
    {
        protected readonly IVuberController _vuberController;

        protected VuberHub(IVuberController vuberController)
        {
            _vuberController = vuberController ?? throw new ArgumentNullException(nameof(vuberController));
        }

        public abstract void SeeRides();

        public abstract void SetRating(Rating rating, Guid rideId);
    }
}