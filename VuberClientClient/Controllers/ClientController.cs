using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Geolocation;
using VuberCore.Entities;
using VuberCore.Dto;
using VuberCore.Hubs;
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
        [Route("createOrder")]
        public IActionResult OrderRide([FromQuery] RideOrder rideOrder)
        {
            _hubWrapper.OrderRide(rideOrder);
            return Ok();
        }

        [HttpPost]
        [Route("cancelOrder")]
        public IActionResult CancelOrder()
        {
            _hubWrapper.CancelOrder();
            return Ok();
        }

        [HttpPost]
        [Route("addPaymentCard")]
        public IActionResult AddPaymentCard([FromQuery] string cardData)
        {
            _hubWrapper.AddPaymentCard(cardData);
            return Ok();
        }

        [HttpGet]
        [Route("seeRides")]
        public IActionResult SeeRides()
        {
            return Ok(_hubWrapper.SeeRides());
        }

        [HttpGet]
        [Route("setRating")]
        public IActionResult SetRating([FromQuery] Mark mark, [FromQuery] Guid driverId)
        {
            _hubWrapper.SetRating(mark, driverId);
            return Ok();
        }

        [HttpGet]
        [Route("getDriverRating")]
        public IActionResult GetDriverRating([FromQuery] Guid driverId)
        {
            return Ok(_hubWrapper.GetDriverRating(driverId));
        }
    }
}
