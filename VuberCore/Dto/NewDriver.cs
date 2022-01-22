using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class NewDriver
    {
        public NewDriver(string username, string name, RideType maxRideLevel, RideType minRideLevel)
        {
            Username = username;
            Name = name;
            MaxRideLevel = maxRideLevel;
            MinRideLevel = minRideLevel;
        }

        public string Username { get; }
        public string Name { get; }
        public RideType MaxRideLevel { get; }
        public RideType MinRideLevel { get; }
    }
}
