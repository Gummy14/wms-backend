using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;
using Constants = WMS_API.Models.Constants;

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
        public IList<LocationData> GetAllLocations()
        {
            return dBContext.LocationData.Where(x => x.NextEventId == null).ToList();
        }

        [HttpGet("GetLocationById/{locationId}")]
        public LocationData GetLocationById(Guid locationId)
        {
            return dBContext.LocationData.FirstOrDefault(x => x.NextEventId == null && x.LocationId == locationId);
        }
        
        [HttpGet("GetPutawayLocation")]
        public LocationData GetPutawayLocation()
        {
            return dBContext.LocationData.FirstOrDefault(x => x.NextEventId == null && x.ItemId == null);
        }

        [HttpGet("GetLocationHistory/{locationId}")]
        public List<LocationData> GetLocationHistory(Guid locationId)
        {
            return dBContext.LocationData.Where(x => x.LocationId == locationId).ToList();
        }
    }
}
