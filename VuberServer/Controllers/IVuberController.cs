using System;
using VuberCore.Entities;

namespace VuberServer.Controllers
{
    public interface IVuberController
    {
        void SetRating(Rating rating, Guid userId);
    }
}