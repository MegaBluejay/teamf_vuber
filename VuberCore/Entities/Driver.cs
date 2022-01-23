using System;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;
using VuberCore.Tools;

namespace VuberCore.Entities
{
    public class Driver : User
    {
        private RideType _maxRideLevel;
        private RideType _minRideLevel;

        public Driver(RideType minRideLevel, RideType maxRideLevel)
        {
            if (!CheckRideLevels(minRideLevel, maxRideLevel))
            {
                throw new VuberException("Error: min ride level should be less than max ride level");
            }

            MinRideLevel = minRideLevel;
            MaxRideLevel = maxRideLevel;
        }

        public RideType MaxRideLevel
        { 
            get => _maxRideLevel;
            set
            { 
                if (!CheckRideLevels(MinRideLevel, value))
                {
                    throw new VuberException("Error: min ride level should be less than max ride level");
                }

                _maxRideLevel = value;
            } 
        }
        public RideType MinRideLevel
        { 
            get => _minRideLevel; 
            set
            {
                if (!CheckRideLevels(value, MaxRideLevel))
                {
                    throw new VuberException("Error: min ride level should be less than max ride level");
                }

                _minRideLevel = value;
            }
        }
        public Point LastKnownLocation { get; private set; }
        public DateTime LocationUpdatedAt { get; private set; }

        public void UpdateLocation(Point newLocation, DateTime updateTime)
        {
            LastKnownLocation = newLocation;
            LocationUpdatedAt = updateTime;
        }

        private bool CheckRideLevels(RideType minRideLevel, RideType maxRideLevel)
        {
            return minRideLevel <= maxRideLevel;
        }
    }
}
