using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
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

        public async Task<List<Location>> GetAllRootLocations()
        {
            var result = await dBContext.Locations
                .Select(x => new Location {
                    Id = x.Id,
                    LocationData = x.LocationData,
                    LocationItem = x.LocationItem,
                    SubLocations = x.SubLocations,
                    LocationParentId = x.LocationParentId
                })
                .Where(x => x.LocationParentId == null)
                .ToListAsync();

            return result;
        }

        public async Task<Location> GetLocationByIdAsync(Guid locationId)
        {
            var result = await dBContext.Locations
                .Include(x => x.LocationData.Where(y => y.NextEventId == null))
                .Include(x => x.LocationItem.Where(y => y.NextEventId == null))
                .FirstOrDefaultAsync(x => x.Id == locationId);

            return result;
        }
        public async Task<List<LocationData>> GetLocationHistoryByIdAsync(Guid locationId)
        {
            var result = await dBContext.LocationData
                .Where(x => x.LocationId == locationId)
                .ToListAsync();

            return result;
        }

        public async Task<LocationData> GetLocationDataByIdAsync(Guid locationId)
        {
            var result = await dBContext.LocationData
                .FirstOrDefaultAsync(x => x.NextEventId == null && x.LocationId == locationId);

            return result;
        }

        public async Task<Location> GetPutawayLocationAsync()
        {
            var result = await dBContext.Locations
                .Include(x => x.LocationData.Where(y => y.NextEventId == null))
                .Include(x => x.LocationItem.Where(y => y.NextEventId == null))
                .Where(x => x.LocationItem.Count == 0 || (x.LocationItem.Count > 0 && x.LocationItem.All(y => y.NextEventId != null)))
                .FirstOrDefaultAsync();

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
