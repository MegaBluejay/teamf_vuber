using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Ride : Entity
    {
        [Required]
        public Client Client { get; set; }
        public Driver Driver { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public RideLevel RideLevel { get; set; }
        [Required]
        public RideStatus Status { get; set; }
        [Required]
        public Point StartLocation { get; set; }
        [Required]
        public ICollection<Point> TargetLocations { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime Found { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
    }
}
