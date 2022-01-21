using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Geolocation;
using VuberCore.Entities;

namespace VuberClientClient.Controllers
{
    [ApiController]
    [Route("/client")]
    public class ClientController
    {
        public ClientController() // сюда нужно передать hub connection
        {
        }

        [HttpPost]
        [Route("createOrder")]
        public IActionResult CreateOrder([FromQuery] Coordinate startLocation, [FromQuery] ICollection<Coordinate> targetLocations, [FromQuery] RideType rideType, PaymentType paymentType)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("cancelOrder")]
        public IActionResult CancelOrder()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("addPayementCard")]
        public IActionResult AddPaymentCard(string cardData)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("seeRides")]
        public IActionResult SeeRides()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("setRating")]
        public IActionResult SetRating([FromQuery] double value, [FromQuery] Guid driverId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("getDriverRating")]
        public IActionResult GetDriverRating(Guid driverId)
        {
            throw new NotImplementedException();
        }
    }
}
