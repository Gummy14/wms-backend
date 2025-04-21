using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;

namespace WMS_API.Layers.Data
{
    public class LocationRepository : ILocationRepository
    {
        private MyDbContext dBContext;

        public LocationRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<List<LocationData>> GetAllLocationsAsync()
        {
            var result = await dBContext.LocationData.Where(x => x.NextEventId == null).ToListAsync();
            return result;
        }

        public async Task<LocationData> GetLocationByIdAsync(Guid locationId)
        {
            var result = await dBContext.LocationData.FirstOrDefaultAsync(x => x.NextEventId == null && x.LocationId == locationId);
            return result;
        }

        public async Task<List<LocationData>> GetLocationHistoryAsync(Guid locationId)
        {
            var result = await dBContext.LocationData.Where(x => x.LocationId == locationId).ToListAsync();
            return result;
        }
        public async Task<LocationData> GetPutawayLocationAsync()
        {
            var result = await dBContext.LocationData.FirstOrDefaultAsync(x => x.NextEventId == null && x.ItemId == null);
            return result;
        }

        public async Task AddLocationAsync(Location location)
        {
            await dBContext.Locations.AddAsync(location);
            await dBContext.SaveChangesAsync();
        }

        public async Task AddLocationDataAsync(LocationData locationData)
        {
            await dBContext.LocationData.AddAsync(locationData);
            await dBContext.SaveChangesAsync();
        }
    }
}
