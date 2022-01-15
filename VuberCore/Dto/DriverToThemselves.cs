using VuberCore.Entities;

namespace VuberCore.Dto
{
    public class DriverToThemselves
    {
        public DriverToThemselves(Driver driver)
        {
            Name = driver.Name;
        }

        public string Name { get; set; }
    }
}