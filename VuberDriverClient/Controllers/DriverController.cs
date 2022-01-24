using System;
using Microsoft.AspNetCore.Mvc;
using VuberCore.Entities;
using VuberCore.Dto;
using VuberCore.Hubs;
using NetTopologySuite.Geometries;
using VuberCore.Hubs;

namespace VuberDriverClient.Controllers
{
    [ApiController]
    [Route("/driver")]
    public class DriverController : ControllerBase
    {
        private IDriverHub _hub;

        public DriverController(IDriverHub hub)
        {
            _hub = hub;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] NewDriver newDriver)
        {
            _hub.Register(newDriver);
            return Ok();
        }

        [HttpGet]
        [Route("see-rides")]
        public IActionResult SeeRides()
        {
            return Ok(_hub.SeeRides());
        }

        [HttpPost]
        [Route("set-rating")]
        public IActionResult SetRating([FromBody] Mark mark, [FromQuery] Guid rideId)
        {
            _hub.SetRating(mark, rideId);
            return Ok();
        }

        [HttpPost]
        [Route("accept-order")]
        public IActionResult AcceptOrder([FromQuery] Guid rideId)
        {
            _hub.AcceptOrder(rideId);
            return Ok();
        }

        [HttpPost]
        [Route("reject-order")]
        public IActionResult RejectOrder([FromQuery] Guid rideId)
        {
            _hub.RejectOrder(rideId);
            return Ok();
        }

        [HttpPost]
        [Route("send-current-location")]
        public IActionResult SendCurrentLocation([FromBody] Point point)
        {
            _hub.SendCurrentLocation(point);
            return Ok();
        }
    }
}
