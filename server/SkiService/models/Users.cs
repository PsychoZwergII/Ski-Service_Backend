public class Users
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Navigationseigenschaft für Rollen
    public ICollection<Userroles> Userroles { get; set; } = new List<Userroles>();
}
