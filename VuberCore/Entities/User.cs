using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public abstract class User : Entity
    {
        [Required]
        public string Username { get; set; }
        public string Name { get; set; }
        public Rating Rating { get; set; }
        
    }
}