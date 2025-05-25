using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllLocationsMostRecentDataAsync();
        Task<Location> GetLocationByIdAsync(Guid locationId);
        Task<List<LocationData>> GetLocationHistoryByIdAsync(Guid locationId);
        Task<LocationData> GetPutawayLocationAsync();
        Task RegisterLocationAsync(UnregisteredLocation objectToRegister);
        Task PutawayItemIntoLocationAsync(Guid itemId, Guid locationId);

    }
}
