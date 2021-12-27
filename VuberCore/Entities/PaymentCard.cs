using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public class PaymentCard
    {
        [Required]
        public string CardData { get; set; }
    }
}