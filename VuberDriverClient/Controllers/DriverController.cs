using System;
using Microsoft.AspNetCore.Mvc;
using VuberCore.Entities;
using VuberCore.Dto;
using VuberCore.Hubs;
using NetTopologySuite.Geometries;

namespace VuberDriverClient.Controllers
{
    [ApiController]
    [Route("/driver")]
    public class DriverController : ControllerBase
    {
        private IDriverHub _hubWrapper;

        public DriverController(IDriverHub hubWrapper)
        {
            _hubWrapper = hubWrapper;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromQuery] NewDriver newDriver)
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

        [HttpPost]
        [Route("accept-order")]
        public IActionResult AcceptOrder([FromQuery] Guid rideId)
        {
            _hubWrapper.AcceptOrder(rideId);
            return Ok();
        }

        [HttpPost]
        [Route("reject-order")]
        public IActionResult RejectOrder([FromQuery] Guid rideId)
        {
            _hubWrapper.RejectOrder(rideId);
            return Ok();
        }

        [HttpPost]
        [Route("send-current-location")]
        public IActionResult SendCurrentLocation([FromQuery] Point point)
        {
            _hubWrapper.SendCurrentLocation(point);
            return Ok();
        }
    }
}
