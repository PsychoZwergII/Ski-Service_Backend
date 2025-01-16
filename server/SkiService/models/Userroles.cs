public class Userroles
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Users User { get; set; } = null!;
    public string Role { get; set; } = string.Empty;
}
