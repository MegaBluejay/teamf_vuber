using System;
using System.Collections.Generic;
using VuberCore.Entities;

namespace VuberServer.Controllers
{
    public interface IVuberController
    {
        List<Ride> SeeRides(User activeUser);
        void SetRating(Rating rating, Guid userId);
    }
}