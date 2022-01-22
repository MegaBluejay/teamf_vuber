using System;
using System.Collections.Generic;
using VuberCore.Entities;
using Geolocation;

namespace VuberServer.Controllers
{
    public interface IVuberController
    {
        Ride CreateNewRide(
            Guid clientId,
            Coordinate startLocation,
            ICollection<Coordinate> targetLocations,
            PaymentType paymentType,
            RideType rideType);

        bool DriverTakesRide(Guid driverId, Guid rideId);

        void DriverArrives(Guid rideId);

        void RideCompleted(Guid rideId);

        void CancelRide(Guid rideId);

        List<Ride> SeeRides(Guid userId);

        Rating SeeRating(Guid rideId, Func<Ride, User> userGetter);

        void SetRating(Rating rating, Guid rideId, Func<Ride, User> userGetter);

        void AddPaymentCard(Guid clientId, string cardData);

        void UpdateDriverLocation(Guid driverId, Coordinate location);

        //void SendNotification(string notification, Guid userToSendNotificationId)
    }
}