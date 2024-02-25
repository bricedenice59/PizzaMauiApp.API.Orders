using PizzaMauiApp.API.Core.EntityFramework;
using PizzaMauiApp.API.Dtos.Enums;

namespace PizzaMauiApp.API.Orders.Models;

public class OrderStatusUpdate : BaseModel
{
    public required Guid OrderId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required OrderStatus Status { get; set; }
}