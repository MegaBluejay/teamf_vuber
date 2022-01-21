using Microsoft.AspNetCore.SignalR;

namespace VuberServer
{
    public class VuberUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}