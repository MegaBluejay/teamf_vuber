using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;
using VuberCore.Tools;

namespace VuberCore.Entities
{
    public class Ride : Entity
    {
        private List<Checkpoint> _checkpoints;
        private IChronometer _chronometer;
        public Ride(Client client, PaymentType paymentType, decimal cost, RideType rideType, LineString path, IChronometer chronometer)
        {
            Client = client;
            PaymentType = paymentType;
            Cost = cost;
            RideType = rideType;
            _checkpoints = path.Coordinates.Skip(1).Select(coordinate => new Checkpoint(new Point(coordinate))).ToList();
            Status = RideStatus.Looking;
            Path = path;
            _chronometer = chronometer;
            Created = _chronometer.TimeNow();
        }

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
        public IEnumerable<Checkpoint> Checkpoints => _checkpoints;
        [Required]
        public RideStatus Status { get; private set; }
        [Required]
        public LineString Path { get; private set; }
        [Required]
        public DateTime Created { get; private set; }
        public DateTime Found { get; private set; }
        public DateTime Started { get; private set; }
        public DateTime Finished { get; private set; }

        public void DriverTakes(Driver driver)
        {
            if (Status != RideStatus.Looking)
            {
                throw new VuberException("Error: need looking ride status to take order");
            }

            Driver = driver;
            Status = RideStatus.Waiting;
            Found = _chronometer.TimeNow();
        }

        public void DriverArrives(DateTime startedDateTime)
        {
            if (Status != RideStatus.Waiting)
            {
                throw new VuberException("Error: need waiting ride status for driver arriving");
            }

            Status = RideStatus.InProgress;
            Started = startedDateTime;
        }

        public void Finish()
        {
            if (Status != RideStatus.InProgress)
            {
                throw new VuberException("Error: need in progress ride status for completing the order");
            }

            Status = RideStatus.Complete;
            Finished = _chronometer.TimeNow();
        }

        public void Cancel()
        {
            if (Status == RideStatus.Complete || Status == RideStatus.Cancelled)
            {
                throw new VuberException("Error: ride cannot be cancelled after completing or canceling");
            }

            Status = RideStatus.Cancelled;
            Finished = _chronometer.TimeNow();
        }

        public void PassCheckpoint(int checkpointNumber)
        {
            _checkpoints[checkpointNumber].Pass();
        }
    }
}
