using WMS_API.Models.Containers;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IContainerService
    {
        Task<List<ContainerData>> GetAllContainersAsync();
        Task<ContainerData> GetContainerByIdAsync(Guid containerId);
        Task<List<ContainerData>> GetContainerHistoryAsync(Guid containerId);
        Task RegisterContainerAsync(UnregisteredObject objectToRegister);
        Task PickItemIntoContainerAsync(Guid itemId, Guid containerId);

    }
}
