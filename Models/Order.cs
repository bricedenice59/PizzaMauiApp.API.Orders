
using PizzaMauiApp.API.Shared.EntityFramework;

namespace PizzaMauiApp.API.Orders.Models;

public class Order : BaseModel
{
    public Guid UserId { get; set; }
    
    public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>(); 

    public ICollection<OrderStatusUpdate> OrdersStatusHistory { get; set; } = new List<OrderStatusUpdate>();
}