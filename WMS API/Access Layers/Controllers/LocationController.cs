using Microsoft.AspNetCore.Mvc;
using WMS_API.Access_Layers.Attributes;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Controllers
{
    [ApiKeyAuth]
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(
            ILocationService locationService
        )
        {
            _locationService = locationService;
        }

        //GET
        [HttpGet("GetAllLocationsMostRecentData")]
        public async Task<IActionResult> GetAllLocationsMostRecentData()
        {
            try
            {
                var result = await _locationService.GetAllLocationsMostRecentDataAsync();
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

        [HttpGet("GetLocationHistoryById/{locationId}")]
        public async Task<IActionResult> GetLocationHistoryById(Guid locationId)
        {
            try
            {
                var result = await _locationService.GetLocationHistoryByIdAsync(locationId);
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
        
        [HttpPost("PutawayItemIntoLocation/{itemId}/{locationId}")]
        public async Task<IActionResult> PutawayItemIntoLocation(Guid itemId, Guid locationId)
        {
            try
            {
                await _locationService.PutawayItemIntoLocationAsync(itemId, locationId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
