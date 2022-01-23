using NetTopologySuite.Geometries;

namespace VuberCore.Entities
{
    public class Checkpoint
    {
        public Checkpoint(Point coordinate)
        {
            Coordinate = coordinate;
        }

        protected Checkpoint() { }

        public int Id { get; set; }
        public Point Coordinate { get; set; }
        public bool IsPassed { get; set; }

        public void Pass()
        {
            IsPassed = true;
        }
    }
}