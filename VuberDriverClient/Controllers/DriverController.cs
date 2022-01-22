using System;
using Microsoft.AspNetCore.Mvc;
using VuberCore.Entities;
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

        [HttpGet]
        [Route("seeRides")]
        public IActionResult SeeRides()
        {
            return Ok(_hubWrapper.SeeRides());
        }

        [HttpPost]
        [Route("setRating")]
        public IActionResult SetRating([FromQuery] Mark mark, [FromQuery] Guid rideId)
        {
            _hubWrapper.SetRating(mark, rideId);
            return Ok();
        }
        
        [HttpGet]
        [Route("seeOrderDetails")]
        public IActionResult SeeOrderDetails(Guid rideId)
        {
            return Ok(_hubWrapper.SeeOrderDetails(rideId));
        }

        [HttpPost]
        [Route("acceptOrder")]
        public IActionResult AcceptOrder(Guid rideId)
        {
            _hubWrapper.AcceptOrder(rideId);
            return Ok();
        }

        [HttpPost]
        [Route("rejectOrder")]
        public IActionResult RejectOrder(Guid rideId)
        {
            _hubWrapper.RejectOrder(rideId);
            return Ok();
        }
    }
}
