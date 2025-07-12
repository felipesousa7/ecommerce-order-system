using Backend.Models.DTOs.Product;
using Backend.Models.Entities;
using Backend.Repositories;

namespace Backend.Services;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request);
    Task<ProductResponse> UpdateAsync(int id, UpdateProductRequest request);
    Task<ProductResponse> GetByIdAsync(int id);
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<IEnumerable<ProductResponse>> GetAvailableAsync();
    Task DeleteAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        if (await _productRepository.NameExistsAsync(request.Name))
        {
            throw new InvalidOperationException("Já existe um produto com este nome");
        }

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CreatedAt = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);

        return MapToResponse(product);
    }

    public async Task<ProductResponse> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException("Produto não encontrado");
        }

        if (request.Name != null && request.Name != product.Name)
        {
            if (await _productRepository.NameExistsAsync(request.Name))
            {
                throw new InvalidOperationException("Já existe um produto com este nome");
            }
            product.Name = request.Name;
        }

        if (request.Description != null)
        {
            product.Description = request.Description;
        }

        if (request.Price.HasValue)
        {
            product.Price = request.Price.Value;
        }

        _productRepository.Update(product);

        return MapToResponse(product);
    }

    public async Task<ProductResponse> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException("Produto não encontrado");
        }

        return MapToResponse(product);
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToResponse);
    }

    public async Task<IEnumerable<ProductResponse>> GetAvailableAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Where(p => p.IsAvailable).Select(MapToResponse);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException("Produto não encontrado");
        }

        _productRepository.Remove(product);
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            IsAvailable = product.IsAvailable,
            ImageUrl = product.ImageUrl,
            CreatedAt = product.CreatedAt
        };
    }
} 