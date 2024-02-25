using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PizzaMauiApp.API.Dtos;
using PizzaMauiApp.API.Orders.Configuration;
using PizzaMauiApp.RabbitMq.Messages;

namespace PizzaMauiApp.API.Orders.Controllers;

public class OrdersController : BaseApiController
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly RabbitMQConfiguration _configuration;

    public OrdersController(
        IPublishEndpoint publishEndpoint, 
        RabbitMQConfiguration configuration)
    {
        _publishEndpoint = publishEndpoint;
        _configuration = configuration;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderDto? orderDto)
    {
        if (orderDto is null) return BadRequest();
        await _publishEndpoint.Publish<IOrderApiToKitchenMessage>(new
        {
            OrderId = orderDto.Id,
            UserId = orderDto.UserId,
            Items = orderDto.OrderItems,
            CreatedAt = DateTime.Now
        },x=>x.TimeToLive = TimeSpan.FromMinutes(_configuration.MessageTTL));
        return Ok();
    }
}