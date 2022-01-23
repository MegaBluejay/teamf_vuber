using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public class PaymentCard
    {
        public int Id { get; init; }
        [Required]
        public string CardData { get; init; }
    }
}