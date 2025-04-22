using WMS_API.Models.Boxes;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IBoxService
    {
        Task<List<BoxData>> GetAllBoxesAsync();
        Task<BoxData> GetBoxByIdAsync(Guid boxId);
        Task<List<BoxData>> GetBoxHistoryAsync(Guid boxId);
        Task RegisterBoxAsync(UnregisteredObject objectToRegister);
        Task PackItemIntoBoxAsync(Guid itemId, Guid boxId);
    }
}
