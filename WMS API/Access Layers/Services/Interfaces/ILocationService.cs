using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(Guid locationId);
        Task<LocationData> GetPutawayLocationAsync();
        Task RegisterLocationAsync(UnregisteredObject objectToRegister);
        Task PutawayItemIntoLocationAsync(Guid itemId, Guid locationId);

    }
}
