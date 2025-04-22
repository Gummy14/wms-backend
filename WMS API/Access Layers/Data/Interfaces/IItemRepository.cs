using WMS_API.Models.Items;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(Guid itemId);
        Task<ItemData> GetItemDataByIdAsync(Guid itemId);
        Task<List<ItemData>> GetAllItemsInOrderAsync(List<ItemData> itemsInOrder);
        Task AddItemAsync(Item item);
        Task AddItemDataAsync(ItemData itemData);
    }
}
