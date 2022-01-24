using VuberCore.Entities;

namespace VuberClientClient.Dto
{
    public class RideOrderDto
    {
        public LineStringDto Path { get; set; }
        public RideType RideType { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}