using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using VuberCore.Dto;

namespace VuberServer.Clients
{
    public interface IClientClient : IVuberClient
    {
        Task UpdateRide(RideToClient rideToClient);

        Task UpdateDriverLocation(Point coordinate);
    }
}