using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Checkpoint
    {
        public Point Coordinate { get; set; }
        public bool IsPassed { get; set; }
    }
}