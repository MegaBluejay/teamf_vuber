using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Checkpoint
    {
        public int Id { get; set; }
        public Point Coordinate { get; set; }
        public bool IsPassed { get; set; }
    }
}