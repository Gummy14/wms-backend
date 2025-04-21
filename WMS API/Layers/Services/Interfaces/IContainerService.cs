using Microsoft.AspNetCore.Mvc;
using WMS_API.Models.Containers;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IContainerService
    {
        Task<List<ContainerData>> GetAllContainersAsync();
        Task<ContainerData> GetContainerByIdAsync(Guid containerId);
        Task<List<ContainerData>> GetContainerHistoryAsync(Guid containerId);
        Task RegisterContainerAsync(UnregisteredObject objectToRegister);
        Task AddContainerToOrderAsync(Guid orderId, Guid containerId);
        Task RemoveContainerFromOrderAsync(Guid containerId);
    }
}
