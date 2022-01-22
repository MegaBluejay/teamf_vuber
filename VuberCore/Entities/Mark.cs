namespace VuberCore.Entities
{
    public struct Mark
    {
        public Mark(double value)
        {
            if (value > 5.0 || value < 0.0)
            {
                throw new System.Exception("Error: mark must be from 0 to 5");
            }

            Value = value;
        }

        public double Value { get; }
    }
}
