using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NetTopologySuite.Geometries;
using VuberCore.Tools;

namespace VuberCore.Entities
{
    public class Ride : Entity
    {
        public Ride(Client client, PaymentType paymentType, decimal cost, RideType rideType, LineString path, IChronometer chronometer)
        {
            Client = client;
            PaymentType = paymentType;
            Cost = cost;
            RideType = rideType;
            Checkpoints = path.Coordinates.Skip(1).Select(coordinate => new Checkpoint(new Point(coordinate))).ToList();
            Status = RideStatus.Looking;
            Path = path;
            Created = chronometer.TimeNow();
        }

        protected Ride() { }

        [Required]
        public virtual Client Client { get; private set; }
        public virtual Driver Driver { get; private set; }
        [Required]
        public decimal Cost { get; private set; }
        [Required]
        public PaymentType PaymentType { get; private set; }
        [Required]
        public RideType RideType { get; private set; }
        [Required]
        public virtual List<Checkpoint> Checkpoints { get; private set; }
        [Required]
        public RideStatus Status { get; private set; }
        [Required]
        public LineString Path { get; private set; }
        [Required]
        public DateTime Created { get; private set; }
        public DateTime Found { get; private set; }
        public DateTime Started { get; private set; }
        public DateTime Finished { get; private set; }

        public void DriverTakes(Driver driver, IChronometer chronometer)
        {
            if (Status != RideStatus.Looking)
            {
                throw new VuberException("Error: need looking ride status to take order");
            }

            Driver = driver;
            Status = RideStatus.Waiting;
            Found = chronometer.TimeNow();
        }

        public void DriverArrives(IChronometer chronometer)
        {
            if (Status != RideStatus.Waiting)
            {
                throw new VuberException("Error: need waiting ride status for driver arriving");
            }

            Status = RideStatus.InProgress;
            Started = chronometer.TimeNow();
        }

        public void Finish(IChronometer chronometer)
        {
            if (Status != RideStatus.InProgress)
            {
                throw new VuberException("Error: need in progress ride status for completing the order");
            }

            Status = RideStatus.Complete;
            Finished = chronometer.TimeNow();
        }

        public void Cancel(IChronometer chronometer)
        {
            if (Status == RideStatus.Complete || Status == RideStatus.Cancelled)
            {
                throw new VuberException("Error: ride cannot be cancelled after completing or canceling");
            }

            Status = RideStatus.Cancelled;
            Finished = chronometer.TimeNow();
        }

        public void PassCheckpoint(int checkpointNumber)
        {
            Checkpoints[checkpointNumber].Pass();
        }
    }
}
