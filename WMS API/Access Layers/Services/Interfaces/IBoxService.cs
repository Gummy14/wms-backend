using WMS_API.Models.Boxes;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IBoxService
    {
        Task<List<Box>> GetAllBoxesAsync();
        Task<Box> GetBoxByIdAsync(Guid boxId);
        Task RegisterBoxAsync(UnregisteredObject objectToRegister);
        Task PackItemIntoBoxAsync(Guid itemId, Guid boxId);
    }
}
