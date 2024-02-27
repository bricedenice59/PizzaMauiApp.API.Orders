
using PizzaMauiApp.API.Core.EntityFramework;

namespace PizzaMauiApp.API.Orders.Models;

public class Order : BaseModel
{
    public required string UserId { get; set; } 
    
    public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>(); 

    public ICollection<OrderStatusUpdate> OrdersStatusHistory { get; set; } = new List<OrderStatusUpdate>();
}