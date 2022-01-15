using System.Threading.Tasks;
using VuberCore.Dto;

namespace VuberServer.Clients
{
    public interface IDriverClient : IVuberClient
    {
        Task RideRequested(RideToDriver ride);
    }
}