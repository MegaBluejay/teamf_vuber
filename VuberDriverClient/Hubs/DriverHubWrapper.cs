using System;
using System.Collections.Generic;
using Geolocation;
using Microsoft.AspNetCore.SignalR.Client;
using VuberCore.Dto;
using VuberCore.Entities;
using VuberCore.Hubs;

namespace VuberDriverClient.Hubs
{
    public class DriverHubWrapper : IDriverHub
    {
        private readonly HubConnection _hubConnection;

        public DriverHubWrapper(HubConnection hubConnection)
        {
            _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));
        }

        public void SetRating(Rating rating, Guid rideId)
        {
            _hubConnection.InvokeAsync(nameof(SetRating), rating, rideId);
        }

        public IEnumerable<RideToDriver> SeeRides()
        {
            return _hubConnection.InvokeAsync<IEnumerable<RideToDriver>>(nameof(SeeRides)).Result;
        }

        public bool AcceptOrder(Guid rideId)
        {
            return _hubConnection.InvokeAsync<bool>(nameof(AcceptOrder), rideId).Result;
        }

        public void RejectOrder(Guid rideId)
        {
            _hubConnection.InvokeAsync(nameof(RejectOrder), rideId);
        }

        public void NotifyClientAboutArrival(Guid rideId)
        {
            _hubConnection.InvokeAsync(nameof(NotifyClientAboutArrival), rideId);
        }

        public void SendCurrentLocation(Coordinate currentLocation)
        {
            _hubConnection.InvokeAsync(nameof(SendCurrentLocation), currentLocation);
        }
    }
}