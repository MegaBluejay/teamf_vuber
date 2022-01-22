using System;
using System.Collections.Generic;
using Geolocation;
using Microsoft.AspNetCore.SignalR.Client;
using VuberClientClient.Controllers;
using VuberCore.Dto;
using VuberCore.Entities;
using VuberCore.Hubs;

namespace VuberClientClient.Hubs
{
    public class ClientHubWrapper : IClientHub
    {
        private readonly HubConnection _hubConnection;

        public ClientHubWrapper(HubConnection hubConnection, IClientNotificationController clientNotificationController)
        {
            _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));
            _hubConnection.On<RideToClient>("UpdateRide", clientNotificationController.UpdateRide);
            _hubConnection.On<Coordinate>("UpdateDriverLocation", clientNotificationController.UpdateDriverLocation);
        }

        public void SetRating(Mark mark, Guid rideId) => _hubConnection.InvokeAsync(nameof(SetRating), mark, rideId);

        public void OrderRide(RideOrder rideOrder) => _hubConnection.InvokeAsync(nameof(OrderRide), rideOrder);

        public void AddPaymentCard(string cardData) => _hubConnection.InvokeAsync(nameof(AddPaymentCard), cardData);

        public IEnumerable<RideToClient> SeeRides() => _hubConnection.InvokeAsync<IEnumerable<RideToClient>>(nameof(SeeRides)).Result;

        public void CancelOrder()
        {
            throw new NotImplementedException();
        }

        public Rating GetDriverRating(Guid driverGuid)
        {
            throw new NotImplementedException();
        }
    }
}