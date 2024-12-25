using System.ComponentModel.DataAnnotations;

namespace HairSalon.API.Models;

public class Appointment
{
    public int Id { get; set; }
    
    [Required]
    public DateTime AppointmentTime { get; set; }
    
    [Required]
    public string ServiceType { get; set; } = string.Empty;
    
    [Required]
    public decimal Price { get; set; }
    
    public string? Notes { get; set; }
    
    public string Status { get; set; } = "Available";
    
    // Foreign keys and navigation properties
    public int? ClientId { get; set; }
    public User? Client { get; set; }
    
    public int StylistId { get; set; }
    public User Stylist { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}