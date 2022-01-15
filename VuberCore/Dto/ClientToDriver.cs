using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class ClientToDriver
    {
        public ClientToDriver(Client client)
        {
            Name = client.Name;
            Rating = client.Rating;
        }

        public string Name { get; set; }
        public Rating Rating { get; set; }
    }
}