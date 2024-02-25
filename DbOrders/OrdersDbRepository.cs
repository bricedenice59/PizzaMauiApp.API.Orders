using PizzaMauiApp.API.Shared.EntityFramework;

namespace PizzaMauiApp.API.Orders.DbOrders;

public class OrdersDbRepository<T>(OrdersDbContext ordersDbContext) : GenericRepository<T>(ordersDbContext)
    where T : BaseModel;