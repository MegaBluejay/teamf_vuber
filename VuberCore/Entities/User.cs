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
        public virtual Rating Rating { get; set; } = new Rating();

        public virtual List<Ride> Rides { get; set; } = new List<Ride>();
    }
}