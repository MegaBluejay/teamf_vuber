using System.Collections.Generic;
using Geolocation;
using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class RideToClient
    {
        public RideToClient(Ride ride)
        {
            StartLocation = ride.StartLocation;
            TargetLocations = ride.TargetLocations;
            RideType = ride.RideType;
            Status = ride.Status;
            Me = new ClientToThemselves(ride.Client);
            Driver = new DriverToClient(ride.Driver);
            Cost = ride.Cost;
        }

        public Coordinate StartLocation { get; set; }
        public ICollection<Coordinate> TargetLocations { get; set; }
        public RideType RideType { get; set; }
        public RideStatus Status { get; set; }
        public ClientToThemselves Me { get; set; }
        public DriverToClient Driver { get; set; }
        public decimal Cost { get; set; }
    }
}