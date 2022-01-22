using System;
using NetTopologySuite.Geometries;

namespace VuberServer.Strategies.CalculateRideDistanceStrategies
{
    public class LinearLengthStrategy : ICalculateLengthStrategy
    {
        public decimal Calculate(Point startLocation, Point endLocation)
        {
            return Convert.ToDecimal(
                Math.Sqrt(Math.Pow(endLocation.X - startLocation.X, 2)
                          + Math.Pow(endLocation.Y - startLocation.Y, 2)));
        }
    }
}