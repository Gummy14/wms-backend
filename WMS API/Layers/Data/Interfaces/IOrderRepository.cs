using WMS_API.Models.Orders;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderData>> GetAllOrdersAsync();
        Task<OrderData> GetOrderByIdAsync(Guid orderId);
        Task<List<OrderData>> GetOrderHistoryAsync(Guid orderId);
        Task AddOrderAsync(Order order);
        Task AddOrderDataAsync(OrderData orderData);
    }
}
