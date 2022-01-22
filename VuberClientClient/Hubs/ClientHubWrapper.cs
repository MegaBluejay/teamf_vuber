using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using VuberCore.Dto;
using VuberCore.Entities;
using VuberCore.Hubs;

namespace VuberClientClient.Hubs
{
    public class ClientHubWrapper : IClientHub
    {
        private readonly HubConnection _hubConnection;

        public ClientHubWrapper(HubConnection hubConnection) => _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));

        public void SetRating(Rating rating, Guid rideId) => _hubConnection.InvokeAsync(nameof(SetRating), rating, rideId);

        public void OrderRide(RideOrder rideOrder) => _hubConnection.InvokeAsync(nameof(OrderRide), rideOrder);

        public void AddPaymentCard(string cardData) => _hubConnection.InvokeAsync(nameof(AddPaymentCard), cardData);

        public IEnumerable<RideToClient> SeeRides() => _hubConnection.InvokeAsync<IEnumerable<RideToClient>>(nameof(SeeRides)).Result;
    }
}