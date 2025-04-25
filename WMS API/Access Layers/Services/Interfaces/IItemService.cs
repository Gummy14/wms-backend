using WMS_API.Models.Items;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<Item>> GetAllItemsMostRecentDataAsync();
        Task<Item> GetItemByIdAsync(Guid itemId);
        Task RegisterItemAsync(UnregisteredObject objectToRegister);
    }
}
