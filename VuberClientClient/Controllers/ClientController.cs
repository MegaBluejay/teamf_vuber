using Microsoft.AspNetCore.Mvc;

namespace VuberClientClient.Controllers
{
    [ApiController]
    [Route("/client")]
    public class ClientController
    {
        public ClientController() // сюда нужно передать hub connection
        {
        }
    }
}
