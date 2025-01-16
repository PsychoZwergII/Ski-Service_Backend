public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = "Offen"; // Default status
    public int ServiceId { get; set; }
    public Service? Service { get; set; } // Optional machen
    public DateTime create_date { get; set; } = DateTime.Now;
    public DateTime pickup_date { get; set; } // Neue Spalte
    public bool IsDeleted { get; set; } = false; // For AO6: Soft Delete
}
