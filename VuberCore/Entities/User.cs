using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VuberCore.Entities
{
    [Index(nameof(Username))]
    public abstract class User : Entity
    {
        [Required]
        public string Username { get; set; }
    }
}
