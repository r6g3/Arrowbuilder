using System.Collections.Generic;

namespace Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    // Arrow ist internal; das Dictionary darf nicht public sein, sonst CS0052.
    // Daher internal setzen. Als Property mit nur Getter, initialisiert.
    internal Dictionary<string, Arrow> arrowPatches { get; } = new Dictionary<string, Arrow>();
    public User(string Name, string Email,string Password)
    {
        this.Name = Name;
        this.Email = Email;
        this.Password = Password;
    }
}