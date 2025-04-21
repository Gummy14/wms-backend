using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoxController : ControllerBase
    {
        private readonly IBoxService _boxService;

        public BoxController(IBoxService boxService)
        {
            _boxService = boxService;
        }

        //GET
        [HttpGet("GetAllBoxes")]
        public async Task<IActionResult> GetAllBoxes()
        {
            try
            {
                await _boxService.GetAllBoxesAsync();
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetBoxById/{boxId}")]
        public async Task<IActionResult> GetBoxById(Guid boxId)
        {
            try
            {
                await _boxService.GetBoxByIdAsync(boxId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        //POST
        [HttpPost("RegisterBox")]
        public async Task<IActionResult> RegisterBox(UnregisteredObject objectToRegister)
        {
            try
            {
                await _boxService.RegisterBoxAsync(objectToRegister);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("AddBoxToOrder/{orderId}/{boxId}")]
        public async Task<IActionResult> AddBoxToOrder(Guid orderId, Guid boxId)
        {
            try
            {
                await _boxService.AddBoxToOrderAsync(orderId, boxId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("AddBoxToShipment/{boxId}")]
        public async Task<IActionResult> AddBoxToShipment(Guid boxId)
        {
            try
            {
                await _boxService.AddBoxToShipmentAsync(boxId);
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
                await _boxService.AddBoxToTruckAsync(boxId, truckId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
