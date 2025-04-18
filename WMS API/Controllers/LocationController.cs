using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;

        public LocationController(MyDbContext context)
        {
            dBContext = context;
            controllerFunctions = new ControllerFunctions();
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

        //POST
        [HttpPost("RegisterLocation")]
        public async Task<StatusCodeResult> RegisterLocation(UnregisteredObject objectToRegister)
        {
            Guid locationId = Guid.NewGuid();

            LocationData newLocationData = new LocationData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                objectToRegister.WeightOrMaxWeightInKilograms,
                locationId,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Location newLocation = new Location(
                locationId,
                new List<LocationData>() { newLocationData },
                null
            );

            dBContext.Locations.Add(newLocation);
            await dBContext.SaveChangesAsync();
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + locationId);
            return StatusCode(200);
        }
    }
}
