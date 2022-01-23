using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public class Rating
    {
        public int Id { get; init; }
        [Required]
        public virtual Mark Value { get; set; } = new Mark(5);

        public uint RidesNumber { get; set; } = 0;
    }
}