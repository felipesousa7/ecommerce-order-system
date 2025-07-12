using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "User";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Relacionamento com pedidos
    public ICollection<Order> Orders { get; set; } = new List<Order>();
} 