using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderData>> GetAllOrdersAsync();
        Task<OrderData> GetOrderByIdAsync(Guid orderId);
        Task<List<OrderData>> GetOrderHistoryAsync(Guid orderId);
        Task RegisterOrderAsync(UnregisteredObject objectToRegister);
    }
}
