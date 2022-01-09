using System;

namespace VuberCore.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
    }
}