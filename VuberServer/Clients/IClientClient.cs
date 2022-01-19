using System.Threading.Tasks;
using Geolocation;
using VuberCore.Dto;

namespace VuberServer.Clients
{
    public interface IClientClient : IVuberClient
    {
        Task UpdateRide(RideToClient rideToClient);

        Task UpdateDriverLocation(Coordinate coordinate);
    }
}