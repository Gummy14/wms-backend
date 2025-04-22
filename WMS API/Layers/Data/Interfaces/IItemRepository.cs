using Microsoft.AspNetCore.Mvc;
using WMS_API.Models.Items;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IItemRepository
    {
        Task<List<ItemData>> GetAllItemsAsync();
        Task<ItemData> GetItemByIdAsync(Guid itemId);
        Task<List<ItemData>> GetItemHistoryAsync(Guid itemId);
        Task<List<ItemData>> GetAllItemsInOrderAsync(List<ItemData> itemsInOrder);
        Task AddItemAsync(Item item);
        Task AddItemDataAsync(ItemData itemData);
    }
}
