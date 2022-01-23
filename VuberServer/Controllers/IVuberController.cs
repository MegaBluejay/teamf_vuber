using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using VuberCore.Entities;

namespace VuberServer.Controllers
{
    public interface IVuberController
    {
        Ride CreateNewRide(
            string clientUsername,
            LineString path,
            PaymentType paymentType,
            RideType rideType);

        bool DriverTakesRide(string driverUsername, Guid rideId);

        void DriverArrives(Guid rideId);

        void PassCheckpoint(Guid rideId, int checkpointNumber);
        void RideCompleted(Guid rideId);

        void CancelRide(Guid rideId);

        List<Ride> SeeRides(string userUsername);

        void SetRating(Mark rating, Guid rideId, Func<Ride, User> userGetter);

        void AddPaymentCard(string clientUsername, string cardData);

        void UpdateDriverLocation(string driverUsername, Point location);

        //void SendNotification(string notification, Guid userToSendNotificationId)
    }
}