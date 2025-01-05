using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.Locations;

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
    }
}
