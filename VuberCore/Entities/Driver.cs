using System;
using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Driver : User
    {
        public RideLevel MaxRideLevel { get; set; }
        public RideLevel MinRideLevel { get; set; }
        public Point LastKnownLocation { get; set; }
        public bool CurrentlyActive { get; set; }
        public DateTime LocationUpdatedAt { get; set; }
    }
}