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

    }
}
