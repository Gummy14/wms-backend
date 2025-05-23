﻿using WMS_API.Models.Containers;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IContainerRepository
    {
        Task<List<Container>> GetAllContainersMostRecentDataAsync();
        Task<Container> GetContainerByIdAsync(Guid containerId);
        Task<List<ContainerData>> GetContainerHistoryByIdAsync(Guid containerId);
        Task<ContainerData> GetContainerDataByIdAsync(Guid containerId);
        Task AddContainerAsync(Container container);
        Task AddContainerDataAsync(ContainerData containerData);
    }
}
