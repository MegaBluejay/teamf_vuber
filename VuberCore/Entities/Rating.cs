using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public class Rating
    {
        [Required]
        public Mark Value { get; set; } 

        public uint RidesNumber { get; set; }
    }
}