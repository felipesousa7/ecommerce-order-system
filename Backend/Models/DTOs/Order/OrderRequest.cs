using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs.Order;

public class OrderItemRequest
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class CreateOrderRequest
{
    [Required]
    [MinLength(1)]
    public List<OrderItemRequest> Items { get; set; } = new();
} 