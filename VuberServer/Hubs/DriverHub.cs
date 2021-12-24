using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public class DriverHub : VuberHub<IDriverClient>
    {
        public DriverHub(IVuberController vuberController)
            : base(vuberController) { }
    }
}