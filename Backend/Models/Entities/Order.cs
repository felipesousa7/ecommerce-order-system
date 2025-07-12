using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Entities;

public enum OrderStatus
{
    Received,
    AwaitingPayment,
    PaymentApproved,
    PaymentRejected,
    StockReserved,
    StockCancelled,
    Error
}

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Received;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Relacionamentos
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
} 