using WMS_API.Models.Containers;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IContainerService
    {
        Task<List<Container>> GetAllContainersMostRecentDataAsync();
        Task<Container> GetContainerByIdAsync(Guid containerId);
        Task RegisterContainerAsync(UnregisteredObject objectToRegister);
        Task<Order> PickItemIntoContainerAsync(Guid itemId, Guid containerId);

    }
}
