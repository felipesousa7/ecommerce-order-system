using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs.Product;

public class CreateProductRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }
}

public class UpdateProductRequest
{
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }

    public string? ImageUrl { get; set; }
} 