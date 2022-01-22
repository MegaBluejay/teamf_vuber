using System;
using System.Collections.Generic;
using System.Linq;
using Geolocation;

namespace VuberServer.Strategies.CalculateRideDistanceStrategies
{
    public class LinearRideDistanceStrategy : ICalculateRideDistanceStrategy
    {
        public decimal Calculate(Coordinate startLocation, IReadOnlyList<Coordinate> targetLocations)
        {
            double length = Math.Sqrt(Math.Pow(startLocation.Latitude - targetLocations.First().Latitude, 2)
                                      + Math.Pow(startLocation.Longitude - targetLocations.First().Longitude, 2));
            if (targetLocations.Count > 1)
            {
                List<Coordinate> listOfTargetLocations = targetLocations.ToList();
                for (int i = 0; i < listOfTargetLocations.Count - 1; i++)
                {
                    Coordinate location1 = listOfTargetLocations[i];
                    Coordinate location2 = listOfTargetLocations[i + 1];
                    length += Math.Sqrt(Math.Pow(location1.Latitude - location2.Latitude, 2)
                                        + Math.Pow(location1.Longitude - location2.Longitude, 2));
                }
            }

            return Convert.ToDecimal(length);
        }
    }
}