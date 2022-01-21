using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class DriverToClient
    {
        public DriverToClient(Driver driver)
        {
            Name = driver.Name;
            Rating = driver.Rating;
        }

        public string Name { get; set; }
        public Rating Rating { get; set; }
    }
}