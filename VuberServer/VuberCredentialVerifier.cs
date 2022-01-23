using System.Linq;
using System.Threading.Tasks;
using Bazinga.AspNetCore.Authentication.Basic;
using VuberServer.Data;

namespace VuberServer
{
    public class VuberCredentialVerifier : IBasicCredentialVerifier
    {
        private VuberDbContext _db;

        public VuberCredentialVerifier(VuberDbContext db)
        {
            _db = db;
        }

        public Task<bool> Authenticate(string username, string password)
        {
            return Task.FromResult(_db.Clients.FirstOrDefault(user => user.Username == username) != null);
        }
    }
}