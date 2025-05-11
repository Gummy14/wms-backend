using WMS_API.Models.Boxes;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IBoxService
    {
        Task<List<Box>> GetAllBoxesMostRecentDataAsync();
        Task<Box> GetBoxByIdAsync(Guid boxId);
        Task RegisterBoxAsync(UnregisteredObject objectToRegister);
        Task<Order> PackItemIntoBoxAsync(Guid itemId, Guid boxId);
    }
}
