using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        [Required]
        public virtual Mark Value { get; set; }

        public uint RidesNumber { get; set; }
    }
}