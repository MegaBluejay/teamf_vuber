using System;

namespace VuberCore.Entities
{
    public class RealTimeChronometer : IChronometer
    {
        public DateTime TimeNow()
        {
            return DateTime.UtcNow;
        }
    }
}
