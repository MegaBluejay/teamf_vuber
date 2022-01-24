using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public class PaymentCard : Entity
    {
        [Required]
        public string CardData { get; init; }
        [Required]
        public decimal Money { get; set; }
    }
}