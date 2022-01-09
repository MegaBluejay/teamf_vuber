using System.Collections.Generic;
using Geolocation;
using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class RideToDriver
    {
        public Coordinate StartLocation { get; set; }
        public ICollection<Coordinate> TargetLocations { get; set; }
        public RideLevel RideLevel { get; set; }
        public RideStatus Status { get; set; }
        public Rating ClientRating { get; set; }
    }
}