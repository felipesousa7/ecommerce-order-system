using Backend.Models.DTOs.Product;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAvailable()
    {
        var products = await _productService.GetAvailableAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request)
    {
        var product = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductResponse>> Update(int id, [FromBody] UpdateProductRequest request)
    {
        var product = await _productService.UpdateAsync(id, request);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
} 