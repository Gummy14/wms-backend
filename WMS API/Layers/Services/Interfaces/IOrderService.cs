using WMS_API.Models.Orders;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderData>> GetAllOrdersAsync();
        Task<OrderData> GetOrderByIdAsync(Guid orderId);
        Task<List<OrderData>> GetOrderHistoryAsync(Guid orderId);
        Task RegisterOrderAsync(UnregisteredOrder unregisteredOrder);
        Task AddContainerToOrderAsync(Guid orderId, Guid containerId);
        Task AddBoxToOrderAsync(Guid orderId, Guid boxId);
        Task RemoveContainerFromOrderAsync(Guid containerId);
    }
}
