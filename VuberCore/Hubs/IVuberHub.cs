using System;
using VuberCore.Entities;

namespace VuberCore.Hubs
{
    public interface IVuberHub
    {
        void SetRating(Rating rating, Guid rideId);
    }
}