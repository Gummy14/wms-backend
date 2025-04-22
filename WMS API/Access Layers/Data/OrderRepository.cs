using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;

namespace WMS_API.Layers.Data
{
    public class OrderRepository : IOrderRepository
    {
        private MyDbContext dBContext;

        public OrderRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var result = await dBContext.Orders
                .Include(x => x.OrderDataHistory)
                .Include(x => x.OrderItems)
                .Include(x => x.Address)
                .Include(x => x.ContainerUsedToPickOrder)
                .Include(x => x.BoxUsedToPackOrder)
                .ToListAsync();

            return result;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            var result = await dBContext.Orders
                .Include(x => x.OrderDataHistory)
                .Include(x => x.OrderItems)
                .Include(x => x.Address)
                .Include(x => x.ContainerUsedToPickOrder)
                .Include(x => x.BoxUsedToPackOrder)
                .FirstOrDefaultAsync(x => x.Id == orderId);

            return result;
        }

        public async Task<OrderData> GetOrderDataByIdAsync(Guid orderId)
        {
            var result = await dBContext.OrderData
                .FirstOrDefaultAsync(x => x.NextEventId == null && x.OrderId == orderId);

            return result;
        }

        public async Task AddOrderAsync(Order order)
        {
            await dBContext.Orders.AddAsync(order);
            await dBContext.SaveChangesAsync();
        }

        public async Task AddOrderDataAsync(OrderData orderData)
        {
            await dBContext.OrderData.AddAsync(orderData);
            await dBContext.SaveChangesAsync();
        }
    }
}
