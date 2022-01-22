using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using NetTopologySuite.Geometries;
using VuberCore.Dto;
using VuberCore.Entities;
using VuberCore.Hubs;
using VuberDriverClient.Controllers;

namespace VuberDriverClient.Hubs
{
    public class DriverHubWrapper : IDriverHub
    {
        private readonly HubConnection _hubConnection;

        public DriverHubWrapper(HubConnection hubConnection, IDriverNotificationController driverNotificationController)
        {
            _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));
            _hubConnection.On<RideToDriver>("RideRequested", driverNotificationController.AddRideRequested);
            _hubConnection.On("RideCancelled", driverNotificationController.CancelRide);
            _hubConnection.On("TakeCashPayment", driverNotificationController.TakeCashPayment);
        }

        public void SetRating(Mark rating, Guid rideId) => _hubConnection.InvokeAsync(nameof(SetRating), rating, rideId);

        public IEnumerable<RideToDriver> SeeRides() => _hubConnection.InvokeAsync<IEnumerable<RideToDriver>>(nameof(SeeRides)).Result;

        public bool AcceptOrder(Guid rideId) => _hubConnection.InvokeAsync<bool>(nameof(AcceptOrder), rideId).Result;

        public void RejectOrder(Guid rideId) => _hubConnection.InvokeAsync(nameof(RejectOrder), rideId);

        public void NotifyClientAboutArrival(Guid rideId) => _hubConnection.InvokeAsync(nameof(NotifyClientAboutArrival), rideId);

        public void SendCurrentLocation(Point currentLocation) => _hubConnection.InvokeAsync(nameof(SendCurrentLocation), currentLocation);

        public RideToDriver SeeOrderDetails(Guid rideId) => _hubConnection.InvokeAsync<RideToDriver>(nameof(SeeOrderDetails), rideId).Result;
    }
}