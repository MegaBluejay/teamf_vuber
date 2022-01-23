using System.ComponentModel.DataAnnotations;

namespace VuberCore.Entities
{
    public class PaymentCard
    {
        public int Id { get; set; }
        [Required]
        public string CardData { get; set; }
        [Required]
        public decimal Money { get; set; }
    }
}