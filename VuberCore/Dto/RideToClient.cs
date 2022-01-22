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
            Checkpoints = ride.Checkpoints;
            RideType = ride.RideType;
            Status = ride.Status;
            Me = new ClientToThemselves(ride.Client);
            Driver = new DriverToClient(ride.Driver);
            Cost = ride.Cost;
        }

        public Coordinate StartLocation { get; }
        public ICollection<Checkpoint> Checkpoints { get; }
        public RideType RideType { get; }
        public RideStatus Status { get; }
        public ClientToThemselves Me { get; }
        public DriverToClient Driver { get; }
        public decimal Cost { get; }
    }
}