using Backend.Models.Entities;

namespace Backend.Models.DTOs.Order;

public class OrderItemResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

public class OrderResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 