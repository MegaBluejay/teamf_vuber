using System;
using System.ComponentModel.DataAnnotations;
using Geolocation;

namespace VuberCore.Entities
{
    public class Driver : User
    {
        [Required]
        public RideLevel MaxRideLevel { get; set; }
        [Required]
        public RideLevel MinRideLevel { get; set; }
        public Coordinate LastKnownLocation { get; set; }
        public DateTime LocationUpdatedAt { get; set; }
    }
}
