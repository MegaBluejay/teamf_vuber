using VuberServer.Clients;
using VuberServer.Controllers;

namespace VuberServer.Hubs
{
    public class ClientHub : VuberHub<IClientClient>
    {
        public ClientHub(IVuberController vuberController)
            : base(vuberController) { }
    }
}