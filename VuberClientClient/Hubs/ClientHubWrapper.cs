using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using VuberCore.Dto;
using VuberCore.Entities;
using VuberCore.Hubs;

namespace VuberClientClient.Hubs
{
    public class ClientHubWrapper : IClientHub
    {
        private readonly HubConnection _hubConnection;

        public ClientHubWrapper(HubConnection hubConnection)
        {
            _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));
        }

        public void SetRating(Mark mark, Guid rideId)
        {
            _hubConnection.InvokeAsync(nameof(SetRating), mark, rideId);
        }

        public void OrderRide(RideOrder rideOrder)
        {
            _hubConnection.InvokeAsync(nameof(OrderRide), rideOrder);
        }

        public void CancelOrder()
        {
            _hubConnection.InvokeAsync(nameof(CancelOrder));
        }

        public void AddPaymentCard(string cardData)
        {
            _hubConnection.InvokeAsync(nameof(AddPaymentCard), cardData);
        }

        public IEnumerable<RideToClient> SeeRides()
        {
            return _hubConnection.InvokeAsync<IEnumerable<RideToClient>>(nameof(SeeRides)).Result;
        }

        public Rating GetDriverRating(Guid driverId)
        {
            return _hubConnection.InvokeAsync<Rating>(nameof(GetDriverRating), driverId).Result;
        }
    }
}