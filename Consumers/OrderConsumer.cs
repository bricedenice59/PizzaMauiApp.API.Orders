using MassTransit;
using PizzaMauiApp.API.Dtos.Enums;
using PizzaMauiApp.API.Orders.Models;
using PizzaMauiApp.API.Orders.Services;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMauiApp.API.Orders.Consumers
{
    public class OrderConsumer : IConsumer<IKitchenToOrderAPIMessage>
    {
        private readonly IOrderService _orderService;
        
        public OrderConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        public async Task Consume(ConsumeContext<IKitchenToOrderAPIMessage> context)
        {
            var order = new Order
            {
                Id = context.Message.OrderId,
                UserId = context.Message.UserId
            };

            foreach (var item in context.Message.Items)
            {
                order.OrderItems.Add(new OrderItems
                {
                    Id = new Guid(),
                    OrderId = order.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    PizzaId = item.ItemId
                });
            }
            
            order.OrdersStatusHistory.Add(new OrderStatusUpdate
            {
                Id = new Guid(),
                OrderId = order.Id,
                CreatedAt = context.Message.CreatedAt.ToUniversalTime(),
                Status = context.Message.IsAccepted ? OrderStatus.New : OrderStatus.Canceled
            });

            await _orderService.CreateOrder(order);
        }
    }
}
