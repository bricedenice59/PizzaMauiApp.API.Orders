
using PizzaMauiApp.API.Dtos.Enums;
using PizzaMauiApp.API.Shared.EntityFramework;

namespace PizzaMauiApp.API.Orders.Models;

public class OrderStatusUpdate : BaseModel
{
    public required Guid OrderId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required OrderStatus Status { get; set; }
}