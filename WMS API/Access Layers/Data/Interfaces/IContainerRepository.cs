using WMS_API.Models.Containers;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IContainerRepository
    {
        Task<List<Container>> GetAllContainersAsync();
        Task<Container> GetContainerByIdAsync(Guid containerId);
        Task<ContainerData> GetContainerDataByIdAsync(Guid containerId);
        Task AddContainerAsync(Container container);
        Task AddContainerDataAsync(ContainerData containerData);
    }
}
