using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class NewClient
    {
        public NewClient(string username, string name, PaymentCard paymentCard = null)
        {
            Username = username;
            Name = name;
            PaymentCard = paymentCard;
        }

        public string Username { get; }
        public string Name { get; }
        public PaymentCard PaymentCard { get; }
    }
}
