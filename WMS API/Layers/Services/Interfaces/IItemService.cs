using WMS_API.Models.Items;
using WMS_API.Models.WarehouseObjects;
using Microsoft.AspNetCore.Mvc;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemData>> GetAllItemsAsync();
        Task<ItemData> GetItemByIdAsync(Guid itemId);
        Task<List<ItemData>> GetItemHistoryAsync(Guid itemId);
        Task RegisterItemAsync(UnregisteredObject objectToRegister);
        Task PutawayItemAsync(Guid itemId, Guid locationId);
        Task PickItemAsync(Guid itemId, Guid containerId);
        Task PackItemAsync(Guid itemId, Guid boxId);
    }
}
