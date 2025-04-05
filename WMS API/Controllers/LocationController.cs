using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private MyDbContext dBContext;

        public LocationController(MyDbContext context)
        {
            dBContext = context;
        }

        //GET
        [HttpGet("GetAllLocations")]
        public IList<Location> GetAllLocations()
        {
            return dBContext.Locations.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetLocationById/{locationId}")]
        public Location GetLocationById(Guid locationId)
        {
            return dBContext.Locations.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == locationId);
        }
        
        [HttpGet("GetPutawayLocation")]
        public WarehouseObject GetPutawayLocation()
        {
            return dBContext.Locations.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.ItemId == Guid.Empty);
        }

        [HttpGet("GetLocationHistory/{locationId}")]
        public List<Location> GetLocationHistory(Guid locationId)
        {
            List<Location> locationHistory = new List<Location>();
            var allLocationEvents = dBContext.Locations.Where(x => x.Id == locationId);
            var firstLocationEvent = allLocationEvents.FirstOrDefault(x => x.PreviousEventId == Guid.Empty);
            locationHistory.Add(firstLocationEvent);

            while (locationHistory.LastOrDefault().NextEventId != Guid.Empty)
            {
                var nextEvent = allLocationEvents.FirstOrDefault(x => x.EventId == locationHistory.LastOrDefault().NextEventId);
                locationHistory.Add(nextEvent);
            }
            return locationHistory;
        }
    }
}
