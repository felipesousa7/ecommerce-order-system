namespace Backend.Models.DTOs.Product;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
} 