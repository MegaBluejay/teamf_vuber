using System;
using Geolocation;

namespace VuberServer.Strategies.CalculateRideDistanceStrategies
{
    public class LinearLengthStrategy : ICalculateLengthStrategy
    {
        public decimal Calculate(Coordinate startLocation, Coordinate endLocation)
        {
            return Convert.ToDecimal(
                Math.Sqrt(Math.Pow(endLocation.Latitude - startLocation.Latitude, 2) 
                          + Math.Pow(endLocation.Longitude - startLocation.Longitude, 2)));
        }
    }
}