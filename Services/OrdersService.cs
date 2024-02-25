using PizzaMauiApp.API.Orders.DbOrders;
using PizzaMauiApp.API.Orders.Models;

namespace PizzaMauiApp.API.Orders.Services;

public interface IOrderService
{
    public Task<bool> CreateOrder(Order order);
}

public class OrdersService : IOrderService
{
    private readonly OrdersDbRepository<Order> _orderRepository;
    private readonly OrdersDbContext _ordersDbContext;
    private readonly ILogger<OrdersService> _logger;

    public OrdersService(ILogger<OrdersService> logger,
        OrdersDbContext ordersDbContext,
        OrdersDbRepository<Order> orderRepository)
    {
        _logger = logger;
        _ordersDbContext = ordersDbContext;
        _orderRepository = orderRepository;
    }

    public async Task<bool> CreateOrder(Order order)
    {
        try
        {
            _ordersDbContext.Orders.Add(order);
            await _ordersDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            _logger.LogError(e.Message, e);
        }

        return false;
    }
}

