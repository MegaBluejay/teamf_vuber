using System;
using Microsoft.AspNetCore.Mvc;
using VuberCore.Entities;
using VuberCore.Dto;
using VuberDriverClient.Hubs;

namespace VuberDriverClient.Controllers
{
    [ApiController]
    [Route("/driver")]
    public class DriverController : ControllerBase
    {
        private DriverHubWrapper _hubWrapper;

        public DriverController(DriverHubWrapper hubWrapper)
        {
            _hubWrapper = hubWrapper;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(NewDriver newDriver)
        {
            _hubWrapper.Register(newDriver);
            return Ok();
        }

        [HttpGet]
        [Route("see-rides")]
        public IActionResult SeeRides()
        {
            return Ok(_hubWrapper.SeeRides());
        }

        [HttpPost]
        [Route("set-rating")]
        public IActionResult SetRating([FromQuery] Mark mark, [FromQuery] Guid rideId)
        {
            _hubWrapper.SetRating(mark, rideId);
            return Ok();
        }

        [HttpGet]
        [Route("see-order-details")]
        public IActionResult SeeOrderDetails(Guid rideId)
        {
            return Ok(_hubWrapper.SeeOrderDetails(rideId));
        }

        [HttpPost]
        [Route("accept-order")]
        public IActionResult AcceptOrder(Guid rideId)
        {
            _hubWrapper.AcceptOrder(rideId);
            return Ok();
        }

        [HttpPost]
        [Route("reject-order")]
        public IActionResult RejectOrder(Guid rideId)
        {
            _hubWrapper.RejectOrder(rideId);
            return Ok();
        }
    }
}
