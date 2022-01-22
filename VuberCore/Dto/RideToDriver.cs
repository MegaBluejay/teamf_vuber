using System.Collections.Generic;
using Geolocation;
using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class RideToDriver
    {
        public RideToDriver(Ride ride)
        {
            StartLocation = ride.StartLocation;
            TargetLocations = ride.TargetLocations;
            RideType = ride.RideType;
            Status = ride.Status;
            Me = new DriverToThemselves(ride.Driver);
            Client = new ClientToDriver(ride.Client);
            Cost = ride.Cost;
        }

        public Coordinate StartLocation { get; }
        public ICollection<Coordinate> TargetLocations { get; }
        public RideType RideType { get; }
        public RideStatus Status { get; }
        public DriverToThemselves Me { get; }
        public ClientToDriver Client { get; }
        public decimal Cost { get; }
    }
}