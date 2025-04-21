using Microsoft.AspNetCore.Mvc;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        //GET
        [HttpGet("GetAllLocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                var result = await _locationService.GetAllLocationsAsync();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetLocationById/{locationId}")]
        public async Task<IActionResult> GetLocationById(Guid locationId)
        {
            try
            {
                var result = await _locationService.GetLocationByIdAsync(locationId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetLocationHistory/{locationId}")]
        public async Task<IActionResult> GetLocationHistory(Guid locationId)
        {
            try
            {
                var result = await _locationService.GetLocationHistoryAsync(locationId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetPutawayLocation")]
        public async Task<IActionResult> GetPutawayLocation()
        {
            try
            {
                var result = await _locationService.GetPutawayLocationAsync();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        //POST
        [HttpPost("RegisterLocation")]
        public async Task<StatusCodeResult> RegisterLocation(UnregisteredObject objectToRegister)
        {
            try
            {
                await _locationService.RegisterLocationAsync(objectToRegister);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
