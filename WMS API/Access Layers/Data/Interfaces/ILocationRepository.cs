using WMS_API.Models.Items;
using WMS_API.Models.Locations;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllRootLocations();
        Task<Location> GetLocationByIdAsync(Guid locationId);
        Task<List<LocationData>> GetLocationHistoryByIdAsync(Guid locationId);
        Task<LocationData> GetLocationDataByIdAsync(Guid locationId);
        Task<Location> GetPutawayLocationAsync();
        Task AddLocationAsync(Location location);
        Task AddLocationDataAsync(LocationData locationData);
    }
}
