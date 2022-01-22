using System;
using VuberCore.Entities;

namespace VuberCore.Hubs
{
    public interface IVuberHub
    {
        void SetRating(Mark mark, Guid rideId);
    }
}