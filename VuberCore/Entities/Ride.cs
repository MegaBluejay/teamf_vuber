using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Ride : Entity
    {
        [Required]
        public virtual Client Client { get; set; }
        public virtual Driver Driver { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }
        [Required]
        public RideType RideType { get; set; }
        [Required]
        public RideStatus Status { get; set; }
        [Required]
        public LineString Path { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime Found { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
    }
}
