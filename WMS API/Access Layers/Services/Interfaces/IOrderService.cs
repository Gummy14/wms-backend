using WMS_API.Models.Orders;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersMostRecentDataAsync();
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<List<OrderData>> GetOrderHistoryByIdAsync(Guid orderId);
        Task RegisterOrderAsync(UnregisteredOrder unregisteredOrder);
        Task<Order> AddBoxToOrderAsync(Guid orderId, Guid boxId);
    }
}
