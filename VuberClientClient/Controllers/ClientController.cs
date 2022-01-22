using System;
using Microsoft.AspNetCore.Mvc;
using VuberCore.Entities;
using VuberCore.Dto;
using VuberClientClient.Hubs;

namespace VuberClientClient.Controllers
{
    [ApiController]
    [Route("/client")]
    public class ClientController : ControllerBase
    {
        private ClientHubWrapper _hubWrapper;
        public ClientController(ClientHubWrapper hubWrapper)
        {
            _hubWrapper = hubWrapper;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(NewClient newClient)
        {
            _hubWrapper.Register(newClient);
            return Ok();
        }

        [HttpPost]
        [Route("create-order")]
        public IActionResult OrderRide([FromQuery] RideOrder rideOrder)
        {
            _hubWrapper.OrderRide(rideOrder);
            return Ok();
        }

        [HttpPost]
        [Route("cancel-order")]
        public IActionResult CancelOrder()
        {
            _hubWrapper.CancelOrder();
            return Ok();
        }

        [HttpPost]
        [Route("add-payment-card")]
        public IActionResult AddPaymentCard([FromQuery] string cardData)
        {
            _hubWrapper.AddPaymentCard(cardData);
            return Ok();
        }

        [HttpGet]
        [Route("see-rides")]
        public IActionResult SeeRides()
        {
            return Ok(_hubWrapper.SeeRides());
        }

        [HttpGet]
        [Route("set-rating")]
        public IActionResult SetRating([FromQuery] Mark mark, [FromQuery] Guid driverId)
        {
            _hubWrapper.SetRating(mark, driverId);
            return Ok();
        }

        [HttpGet]
        [Route("get-driver-rating")]
        public IActionResult GetDriverRating([FromQuery] Guid driverId)
        {
            return Ok(_hubWrapper.GetDriverRating(driverId));
        }
    }
}
