using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using VuberClientClient.Dto;
using VuberCore.Entities;
using VuberCore.Dto;
using VuberCore.Hubs;

namespace VuberClientClient.Controllers
{
    [ApiController]
    [Route("/client")]
    public class ClientController : ControllerBase
    {
        private IClientHub _hub;
        public ClientController(IClientHub hub)
        {
            _hub = hub;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] NewClient newClient)
        {
            _hub.Register(newClient);
            return Ok();
        }

        [HttpPost]
        [Route("create-order")]
        public IActionResult OrderRide([FromBody] RideOrderDto rideOrder)
        {
            _hub.OrderRide(new RideOrder()
            {
                Path = new LineString(rideOrder.Path.Points.Select(point => new Coordinate(point.X, point.Y)).ToArray()),
                RideType = rideOrder.RideType,
                PaymentType = rideOrder.PaymentType
            });
            return Ok();
        }

        [HttpPost]
        [Route("cancel-order")]
        public IActionResult CancelOrder()
        {
            _hub.CancelOrder();
            return Ok();
        }

        [HttpPost]
        [Route("add-payment-card")]
        public IActionResult AddPaymentCard([FromQuery] string cardData)
        {
            _hub.AddPaymentCard(cardData);
            return Ok();
        }

        [HttpGet]
        [Route("see-rides")]
        public IActionResult SeeRides()
        {
            return Ok(_hub.SeeRides());
        }

        [HttpGet]
        [Route("set-rating")]
        public IActionResult SetRating([FromBody] Mark mark, [FromQuery] Guid driverId)
        {
            _hub.SetRating(mark, driverId);
            return Ok();
        }

        [HttpGet]
        [Route("get-driver-rating")]
        public IActionResult GetDriverRating([FromQuery] Guid driverId)
        {
            return Ok(_hub.GetDriverRating(driverId));
        }
    }
}
