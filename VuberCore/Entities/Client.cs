using System.Collections.Generic;

namespace VuberCore.Entities
{
    public class Client : User
    {
        public PaymentCard PaymentCard { get; set; }
        public List<Ride> Rides;
    }
}