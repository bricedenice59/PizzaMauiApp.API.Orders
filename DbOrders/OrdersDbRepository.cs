
using PizzaMauiApp.API.Core.EntityFramework;

namespace PizzaMauiApp.API.Orders.DbOrders;

public class OrdersDbRepository<T>(OrdersDbContext ordersDbContext) : GenericRepository<T>(ordersDbContext)
    where T : BaseModel;