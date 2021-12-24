using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Ride : Entity
    {
        public Client Client { get; set; }
        public Driver Driver { get; set; }
        public decimal Cost { get; set; }
        public RideLevel RideLevel { get; set; }
        public RideStatus Status { get; set; }
        public Point StartLocation { get; set; }
        public ICollection<Point> TargetLocations { get; set; }
    }
}