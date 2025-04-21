using WMS_API.Models.Containers;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IContainerRepository
    {
        Task<List<ContainerData>> GetAllContainersAsync();
        Task<ContainerData> GetContainerByIdAsync(Guid containerId);
        Task<List<ContainerData>> GetContainerHistoryAsync(Guid containerId);
        Task AddContainerAsync(Container container);
        Task AddContainerDataAsync(ContainerData containerData);
    }
}
