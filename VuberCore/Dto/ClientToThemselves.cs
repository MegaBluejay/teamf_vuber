using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class ClientToThemselves
    {
        public ClientToThemselves(Client client)
        {
            Name = client.Name;
        }

        public string Name { get; }
    }
}