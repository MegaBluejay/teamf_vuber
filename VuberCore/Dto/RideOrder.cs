using System.Collections.Generic;
using Geolocation;
using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class RideOrder
    {
        public Coordinate StartLocation { get; set; }
        public IEnumerable<Coordinate> TargetLocations { get; set; }
        public RideType RideType { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}