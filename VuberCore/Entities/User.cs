using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public abstract class User : Entity
    {
        private List<Ride> _rides = new List<Ride>();

        [Required]
        public string Username { get; init; }
        public string Name { get; set; }
        public virtual Rating Rating { get; init; } = new Rating();

        public virtual IReadOnlyList<Ride> Rides => _rides;

        public void AddRide(Ride ride)
        {
            _rides.Add(ride);
        }
    }
}