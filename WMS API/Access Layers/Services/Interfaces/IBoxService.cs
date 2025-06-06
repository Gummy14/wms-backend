﻿using WMS_API.Models.Boxes;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IBoxService
    {
        Task<List<Box>> GetAllBoxesMostRecentDataAsync();
        Task<Box> GetBoxByIdAsync(Guid boxId);
        Task<List<BoxData>> GetBoxHistoryByIdAsync(Guid boxId);
        Task RegisterBoxAsync(UnregisteredObject objectToRegister);
        Task<Order> PickItemIntoBoxAsync(Guid itemId, Guid boxId);
    }
}
