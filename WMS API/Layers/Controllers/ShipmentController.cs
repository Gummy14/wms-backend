using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Shipments;
using WMS_API.Models.Trucks;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipmentController : ControllerBase
    {
        private MyDbContext dBContext;
        private readonly IShipmentService _shipmentService;

        public ShipmentController(MyDbContext context, IShipmentService shipmentService)
        {
            dBContext = context;
            _shipmentService = shipmentService;
        }

        //GET
        [HttpGet("GetAllShipments")]
        public async Task<IActionResult> GetAllShipments()
        {
            try
            {
                var result = await _shipmentService.GetAllShipmentsAsync();
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

        [HttpGet("GetShipmentHistory/{shipmentId}")]
        public async Task<IActionResult> GetShipmentHistory(Guid shipmentId)
        {
            try
            {
                var result = await _shipmentService.GetShipmentHistoryAsync(shipmentId);
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

        [HttpPost("AddTruckToShipment/{shipmentId}/{truckLicensePlate}")]
        public async Task<IActionResult> AddShipmentToTruck(Guid shipmentId, string truckLicensePlate)
        {
            try
            {
                await _shipmentService.AddShipmentToTruckAsync(shipmentId, truckLicensePlate);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
