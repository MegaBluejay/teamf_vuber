using Geolocation;

namespace VuberCore.Entities
{
    public class Checkpoint
    {
        public Coordinate Coordinate { get; set; }
        public bool IsPassed { get; set; }
    }
}