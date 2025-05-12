using Microsoft.AspNetCore.Mvc;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(
            IShipmentService shipmentService
        )
        {
            _shipmentService = shipmentService;
        }

        //GET
        [HttpGet("GetAllShipmentsMostRecentData")]
        public async Task<IActionResult> GetAllShipmentsMostRecentData()
        {
            try
            {
                var result = await _shipmentService.GetAllShipmentsMostRecentDataAsync();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetShipmentById/{shipmentId}")]
        public async Task<IActionResult> GetShipmentById(Guid shipmentId)
        {
            try
            {
                var result = await _shipmentService.GetShipmentByIdAsync(shipmentId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetShipmentHistoryById/{shipmentId}")]
        public async Task<IActionResult> GetShipmentHistoryById(Guid shipmentId)
        {
            try
            {
                var result = await _shipmentService.GetShipmentHistoryByIdAsync(shipmentId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        //POST
        [HttpPost("RegisterShipment")]
        public async Task<IActionResult> RegisterShipment(UnregisteredObject objectToRegister)
        {
            try
            {
                await _shipmentService.RegisterShipmentAsync(objectToRegister);
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
                await _shipmentService.AddBoxToShipmentAsync(boxId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("AddTruckToShipment/{shipmentId}/{truckLicensePlate}")]
        public async Task<IActionResult> AddTruckToShipment(Guid shipmentId, string truckLicensePlate)
        {
            try
            {
                await _shipmentService.AddTruckToShipmentAsync(shipmentId, truckLicensePlate);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
