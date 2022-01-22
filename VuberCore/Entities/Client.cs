using System.Collections.Generic;

namespace VuberCore.Entities
{
    public class Client : User
    {
        public virtual PaymentCard PaymentCard { get; set; }
    }
}