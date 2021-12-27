namespace VuberCore.Entities
{
    public abstract class User : Entity
    {
        public string Name { get; set; }
        public Rating Rating { get; set; }
    }
}