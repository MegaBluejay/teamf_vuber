using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using VuberCore.Entities;
using VuberCore.Dto;

namespace VuberServer.Controllers
{
    public interface IVuberController
    {
        Ride CreateNewRide(
            Guid clientId,
            LineString path,
            PaymentType paymentType,
            RideType rideType);

        void RegisterClient(NewClient newClient);

        void RegisterDriver(NewDriver newDriver);

        bool DriverTakesRide(Guid driverId, Guid rideId);

        void DriverArrives(Guid rideId);

        void PassCheckpoint(Guid rideId, int checkpointNumber);

        void RideCompleted(Guid rideId);

        void CancelRide(Guid rideId);

        List<Ride> SeeRides(Guid userId);

        void SetRating(Mark rating, Guid rideId, Func<Ride, User> userGetter);

        void AddPaymentCard(Guid clientId, string cardData);

        void UpdateDriverLocation(Guid driverId, Point location);

        //void SendNotification(string notification, Guid userToSendNotificationId)
    }
}