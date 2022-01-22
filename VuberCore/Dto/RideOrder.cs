using NetTopologySuite.Geometries;
using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class RideOrder
    {
        public LineString Path { get; set; }
        public RideType RideType { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}