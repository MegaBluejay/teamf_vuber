using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Checkpoint
    {
        public Checkpoint(Point coordinate)
        {
            Coordinate = coordinate;
        }

        public Point Coordinate { get; init; }
        public bool IsPassed { get; private set; }

        public void Pass()
        {
            IsPassed = true;
        }
    }
}