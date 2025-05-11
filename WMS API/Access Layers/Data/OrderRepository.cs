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

        public async Task<List<Order>> GetAllOrdersMostRecentDataAsync()
        {
            var result = await dBContext.Orders
                .Include(x => x.OrderData.Where(y => y.NextEventId == null))
                .Include(x => x.OrderItems.Where(y => y.NextEventId == null))
                .Include(x => x.Address)
                .Include(x => x.OrderContainer.Where(y => y.NextEventId == null))
                .Include(x => x.OrderBox.Where(y => y.NextEventId == null))
                .ToListAsync();

            return result;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            var result = await dBContext.Orders
                .Include(x => x.OrderData)
                .Include(x => x.OrderItems)
                .Include(x => x.Address)
                .Include(x => x.OrderContainer)
                .Include(x => x.OrderBox)
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
