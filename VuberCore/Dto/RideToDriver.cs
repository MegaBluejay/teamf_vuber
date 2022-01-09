using System.Collections.Generic;
using Geolocation;
using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class RideToDriver
    {
        public Coordinate StartLocation { get; set; }
        public ICollection<Coordinate> TargetLocations { get; set; }
        public RideType RideType { get; set; }
        public RideStatus Status { get; set; }
        public DriverToThemselves Me { get; set; }
        public ClientToDriver Client { get; set; }
        public decimal Payment { get; set; }
    }
}