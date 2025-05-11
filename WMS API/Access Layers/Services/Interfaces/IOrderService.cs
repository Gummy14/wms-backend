using WMS_API.Models.Orders;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersMostRecentDataAsync();
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task RegisterOrderAsync(UnregisteredOrder unregisteredOrder);
        Task<Order> AddContainerToOrderAsync(Guid orderId, Guid containerId);
        Task<Order> AddBoxToOrderAsync(Guid orderId, Guid boxId);
        Task RemoveContainerFromOrderAsync(Guid containerId);
    }
}
