using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PizzaMauiApp.API.Dtos;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMauiApp.API.Orders.Controllers;

public class OrdersController : BaseApiController
{
    private readonly IPublishEndpoint _publishEndpoint;
    
    public OrdersController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;

    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderDto? orderDto)
    {
        if (orderDto is null) return BadRequest();
        await _publishEndpoint.Publish<IOrderMessage>(new
        {
            OrderId = orderDto.Id,
            UserId = orderDto.UserId,
            Items = orderDto.OrderItems
        });
        return Ok();
    }
}