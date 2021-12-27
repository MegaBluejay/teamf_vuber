using System;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Driver : User
    {
        [Required]
        public RideLevel MaxRideLevel { get; set; }
        [Required]
        public RideLevel MinRideLevel { get; set; }
        public Point LastKnownLocation { get; set; }
        public DateTime LocationUpdatedAt { get; set; }
    }
}
