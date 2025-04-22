using WMS_API.Models.Items;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemData>> GetAllItemsAsync();
        Task<ItemData> GetItemByIdAsync(Guid itemId);
        Task<List<ItemData>> GetItemHistoryAsync(Guid itemId);
        Task RegisterItemAsync(UnregisteredObject objectToRegister);
    }
}
