using System.Collections.Generic;
using Geolocation;
using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class RideToClient
    {
        public Coordinate StartLocation { get; set; }
        public ICollection<Coordinate> TargetLocations { get; set; }
        public RideLevel RideLevel { get; set; }
        public RideStatus Status { get; set; }
        public ClientToThemselves Me { get; set; }
        public DriverToClient Driver { get; set; }
        public decimal Cost { get; set; }
    }
}