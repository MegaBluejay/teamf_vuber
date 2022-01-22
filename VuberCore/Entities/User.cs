using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public abstract class User : Entity
    {
        [Required]
        public string Username { get; set; }
        public string Name { get; set; }
        public virtual Rating Rating { get; set; }

        public virtual List<Ride> Rides { get; set; }
    }
}