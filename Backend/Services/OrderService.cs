using Backend.Models.DTOs.Order;
using Backend.Models.Entities;
using Backend.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(int userId, CreateOrderRequest request);
    Task<OrderResponse> GetByIdAsync(int id);
    Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(int userId);
    Task<OrderResponse> UpdateStatusAsync(int id, OrderStatus status);
}

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IPaymentSimulationService _paymentSimulationService;
    private readonly ILogger<OrderService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OrderService(
        IRepository<Order> orderRepository, 
        IRepository<Product> productRepository,
        IPaymentSimulationService paymentSimulationService,
        ILogger<OrderService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _paymentSimulationService = paymentSimulationService;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<OrderResponse> CreateAsync(int userId, CreateOrderRequest request)
    {
        if (request.Items == null || !request.Items.Any())
        {
            throw new InvalidOperationException("O pedido deve conter pelo menos um item");
        }

        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.Received,
            CreatedAt = DateTime.UtcNow,
            Items = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Produto com ID {item.ProductId} não encontrado");
            }

            if (!product.IsAvailable)
            {
                throw new InvalidOperationException($"Produto {product.Name} não está disponível");
            }

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                TotalPrice = product.Price * item.Quantity
            };

            order.Items.Add(orderItem);
            totalAmount += orderItem.TotalPrice;
        }

        order.TotalAmount = totalAmount;
        await _orderRepository.AddAsync(order);

        _logger.LogInformation("Pedido {OrderId} criado com status {Status}", order.Id, order.Status);

        // Inicia o processamento assíncrono do pedido
        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var orderRepository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentSimulationService>();

                // Atualiza para Aguardando Pagamento
                order.Status = OrderStatus.AwaitingPayment;
                order.UpdatedAt = DateTime.UtcNow;
                orderRepository.Update(order);
                _logger.LogInformation("Pedido {OrderId} atualizado para status {Status}", order.Id, order.Status);

                // Simula o pagamento
                var paymentStatus = await paymentService.SimulatePaymentAsync(order.Id);
                
                // Atualiza o status do pedido com base no resultado do pagamento
                order.Status = paymentStatus;
                order.UpdatedAt = DateTime.UtcNow;
                orderRepository.Update(order);
                _logger.LogInformation("Pedido {OrderId} atualizado para status {Status} após simulação de pagamento", order.Id, order.Status);

                // Simula a reserva de estoque
                await paymentService.SimulateStockReservationAsync(order.Id, paymentStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar pedido {OrderId}", order.Id);
                using var errorScope = _serviceScopeFactory.CreateScope();
                var errorOrderRepository = errorScope.ServiceProvider.GetRequiredService<IRepository<Order>>();
                
                order.Status = OrderStatus.Error;
                order.UpdatedAt = DateTime.UtcNow;
                errorOrderRepository.Update(order);
            }
        });

        return await MapToResponseAsync(order);
    }

    public async Task<OrderResponse> GetByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdWithIncludesAsync(id, 
            o => o.User,
            o => o.Items);
            
        if (order == null)
        {
            throw new KeyNotFoundException("Pedido não encontrado");
        }

        // Carrega os produtos para cada item
        foreach (var item in order.Items)
        {
            item.Product = await _productRepository.GetByIdAsync(item.ProductId);
        }

        return MapToResponse(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(int userId)
    {
        var orders = await _orderRepository.FindWithIncludesAsync(
            o => o.UserId == userId,
            o => o.User,
            o => o.Items,
            o => o.Items);
        
        // Carrega os produtos para cada item
        foreach (var order in orders)
        {
            foreach (var item in order.Items)
            {
                item.Product = await _productRepository.GetByIdAsync(item.ProductId);
            }
        }
        
        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponse> UpdateStatusAsync(int id, OrderStatus status)
    {
        var order = await _orderRepository.GetByIdWithIncludesAsync(id,
            o => o.User,
            o => o.Items);
        if (order == null)
        {
            throw new KeyNotFoundException("Pedido não encontrado");
        }

        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;

        _orderRepository.Update(order);

        return MapToResponse(order);
    }

    private OrderResponse MapToResponse(Order order)
    {
        _logger.LogInformation("Mapeando pedido {OrderId} para resposta", order.Id);

        var response = new OrderResponse
        {
            Id = order.Id,
            UserId = order.UserId,
            UserName = order.User?.Name ?? "Usuário não encontrado",
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Items = new List<OrderItemResponse>()
        };

        foreach (var item in order.Items)
        {
            if (item.Product == null)
            {
                _logger.LogWarning("Produto não encontrado para o item {ItemId} do pedido {OrderId}", item.Id, order.Id);
                continue;
            }

            response.Items.Add(new OrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice
            });
        }

        return response;
    }

    private async Task<OrderResponse> MapToResponseAsync(Order order)
    {
        var orderWithDetails = await _orderRepository.GetByIdWithIncludesAsync(order.Id,
            o => o.User,
            o => o.Items);
        if (orderWithDetails == null)
        {
            throw new InvalidOperationException("Erro ao carregar detalhes do pedido");
        }

        return MapToResponse(orderWithDetails);
    }
} 