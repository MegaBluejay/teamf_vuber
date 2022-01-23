using VuberCore.Tools;

namespace VuberCore.Entities
{
    public class Mark
    {
        public int Id { get; set; }
        public Mark(double value)
        {
            if (value is > 5.0 or < 0.0)
            {
                throw new VuberException("Error: mark must be from 0 to 5");
            }

            Value = value;
        }

        protected Mark() { }

        public double Value { get; }
    }
}
