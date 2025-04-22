using WMS_API.Models.Items;
using WMS_API.Models.Locations;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(Guid locationId);
        Task<LocationData> GetLocationDataByIdAsync(Guid locationId);
        Task<LocationData> GetPutawayLocationAsync();
        Task AddLocationAsync(Location location);
        Task AddLocationDataAsync(LocationData locationData);
    }
}
