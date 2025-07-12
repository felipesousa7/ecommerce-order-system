using Backend.Models.Entities;
using Microsoft.Extensions.Logging;

namespace Backend.Services;

public interface IPaymentSimulationService
{
    Task<OrderStatus> SimulatePaymentAsync(int orderId);
    Task SimulateStockReservationAsync(int orderId, OrderStatus status);
}

public class PaymentSimulationService : IPaymentSimulationService
{
    private readonly ILogger<PaymentSimulationService> _logger;
    private readonly Random _random;

    public PaymentSimulationService(ILogger<PaymentSimulationService> logger)
    {
        _logger = logger;
        _random = new Random();
    }

    public async Task<OrderStatus> SimulatePaymentAsync(int orderId)
    {
        // Simula delay de 2 a 5 segundos
        var delay = _random.Next(2000, 5000);
        await Task.Delay(delay);

        // 80% de chance de sucesso
        var isSuccess = _random.NextDouble() < 0.8;

        var status = isSuccess ? OrderStatus.PaymentApproved : OrderStatus.PaymentRejected;
        _logger.LogInformation("Simulação de pagamento para pedido {OrderId}: {Status}", orderId, status);

        return status;
    }

    public Task SimulateStockReservationAsync(int orderId, OrderStatus status)
    {
        if (status == OrderStatus.PaymentApproved)
        {
            _logger.LogInformation("Item reservado em estoque para o pedido {OrderId}", orderId);
        }
        else
        {
            _logger.LogInformation("Cancelando reserva de estoque para o pedido {OrderId}", orderId);
        }

        return Task.CompletedTask;
    }
} 