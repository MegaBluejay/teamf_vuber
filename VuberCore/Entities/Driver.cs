using System;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Driver : User
    {
        [Required]
        public RideType MaxRideLevel { get; set; }
        [Required]
        public RideType MinRideLevel { get; set; }
        public Point LastKnownLocation { get; set; }
        public DateTime LocationUpdatedAt { get; set; }
    }
}
