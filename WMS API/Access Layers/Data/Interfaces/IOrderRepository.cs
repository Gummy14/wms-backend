using WMS_API.Models.Orders;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersMostRecentDataAsync();
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<OrderData> GetOrderDataByIdAsync(Guid orderId);
        Task AddOrderAsync(Order order);
        Task AddOrderDataAsync(OrderData orderData);
    }
}
