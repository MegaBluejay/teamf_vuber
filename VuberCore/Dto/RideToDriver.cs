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

        public Coordinate StartLocation { get; set; }
        public ICollection<Coordinate> TargetLocations { get; set; }
        public RideType RideType { get; set; }
        public RideStatus Status { get; set; }
        public DriverToThemselves Me { get; set; }
        public ClientToDriver Client { get; set; }
        public decimal Cost { get; set; }
    }
}