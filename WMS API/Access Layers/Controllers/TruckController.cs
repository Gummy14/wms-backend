using Microsoft.AspNetCore.Mvc;
using WMS_API.Layers.Services.Interfaces;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TruckController : ControllerBase
    {
        private readonly ITruckService _truckService;

        public TruckController(
            ITruckService truckService
        )
        {
            _truckService = truckService;
        }

        //GET
        [HttpGet("GetAllTrucks")]
        public async Task<IActionResult> GetAllTrucks()
        {
            try
            {
                var result = await _truckService.GetAllTrucksAsync();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        //POST
        [HttpPost("SetTruckDepartedAsync/{truckId}")]
        public async Task<IActionResult> SetTruckDepartedAsync(Guid truckId)
        {
            try
            {
                await _truckService.SetTruckDepartedAsync(truckId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("AddBoxToTruck/{boxId}/{truckId}")]
        public async Task<IActionResult> AddBoxToTruck(Guid boxId, Guid truckId)
        {
            try
            {
                await _truckService.AddBoxToTruckAsync(boxId, truckId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
