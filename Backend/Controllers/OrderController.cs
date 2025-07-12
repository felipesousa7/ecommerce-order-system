using Backend.Models.DTOs.Order;
using Backend.Models.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetUserOrders()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var orders = await _orderService.GetUserOrdersAsync(userId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var order = await _orderService.CreateAsync(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(int id, [FromBody] OrderStatus status)
    {
        var order = await _orderService.UpdateStatusAsync(id, status);
        return Ok(order);
    }
} 