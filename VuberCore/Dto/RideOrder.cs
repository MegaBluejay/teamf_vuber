using System.Collections.Generic;
using Geolocation;
using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class RideOrder
    {
        public RideOrder(
            Coordinate startLocation,
            IEnumerable<Coordinate> targetLocations,
            RideType rideType,
            PaymentType paymentType)
        {
            StartLocation = startLocation;
            TargetLocations = targetLocations;
            RideType = rideType;
            PaymentType = PaymentType;
        }

        public Coordinate StartLocation { get; }
        public IEnumerable<Coordinate> TargetLocations { get; }
        public RideType RideType { get; }
        public PaymentType PaymentType { get; }
    }
}