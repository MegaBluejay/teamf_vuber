using System;
using Microsoft.AspNetCore.Mvc;

namespace VuberDriverClient.Controllers
{
    [ApiController]
    [Route("/driver")]
    public class DriverController : ControllerBase
    {

        public DriverController() // сюда нужно передать hub connection
        {
        }

        [HttpGet]
        [Route("seeRides")]
        public IActionResult SeeRides()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("setRating")]
        public IActionResult SetRating([FromQuery] double value, [FromQuery] Guid clientGuid)
        {
            throw new NotImplementedException();
        }
        
        [HttpGet]
        [Route("seeOrderDetails")]
        public IActionResult SeeOrderDetails()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("acceptOrder")]
        public IActionResult AcceptOrder()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("rejectOrder")]
        public IActionResult RejectOrder()
        {
            throw new NotImplementedException();
        }
    }
}
