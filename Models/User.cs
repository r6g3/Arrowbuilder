using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowbuilder.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Navigation Property für Arrows
        public ICollection<Arrow> Arrows { get; set; } = new List<Arrow>();

        // Parameterloser Constructor für EF Core
        public User()
        {
        }

        // Constructor für normale Verwendung
        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            PasswordHash = password;
        }
    }
}