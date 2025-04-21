using WMS_API.Models.Items;
using WMS_API.Models.Locations;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<LocationData>> GetAllLocationsAsync();
        Task<LocationData> GetLocationByIdAsync(Guid locationId);
        Task<List<LocationData>> GetLocationHistoryAsync(Guid locationId);
        Task<LocationData> GetPutawayLocationAsync();
        Task AddLocationAsync(Location location);
        Task AddLocationDataAsync(LocationData locationData);
    }
}
