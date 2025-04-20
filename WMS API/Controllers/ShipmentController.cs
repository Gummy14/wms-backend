using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WMS_API.DbContexts;
using WMS_API.Models.Boxes;
using WMS_API.Models.Containers;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using WMS_API.Models.Shipment;
using WMS_API.Models.Trucks;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipmentController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;

        public ShipmentController(MyDbContext context)
        {
            dBContext = context;
            controllerFunctions = new ControllerFunctions();
        }

        //GET
        [HttpGet("GetAllShipments")]
        public IList<Shipment> GetAllShipments()
        {
            return dBContext.Shipments
                .Include(x => x.ShipmentDataHistory)
                .Include(x => x.ShipmentBoxes)
                .Include(x => x.TruckData)
                .ToList();
        }

        [HttpGet("GetShipmentById/{shipmentId}")]
        public ShipmentData GetShipmentById(Guid shipmentId)
        {
            return dBContext.ShipmentData.FirstOrDefault(x => x.NextEventId == null && x.ShipmentId == shipmentId);
        }

        //POST
        [HttpPost("RegisterShipment")]
        public async Task<StatusCodeResult> RegisterShipment(UnregisteredObject objectToRegister)
        {
            Guid shipmentId = Guid.NewGuid();

            ShipmentData newShipmentData = new ShipmentData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                shipmentId,
                Guid.NewGuid(),
                null,
                null
            );

            Shipment newShipment = new Shipment(
                shipmentId,
                new List<ShipmentData>() { newShipmentData },
                null,
                null
            );

            dBContext.Shipments.Add(newShipment);
            await dBContext.SaveChangesAsync();
            return StatusCode(200);
        }

        [HttpPost("AddTruckToShipment/{shipmentId}/{truckLicensePlate}")]
        public async Task<Shipment> AddShipmentToTruck(Guid shipmentId, string truckLicensePlate)
        {
            var shipmentToUpdate = dBContext.Shipments.FirstOrDefault(x => x.Id == shipmentId);

            if (shipmentToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid truckId = Guid.NewGuid();

                Truck newTruck = new Truck(
                    truckId,
                    shipmentToUpdate.Id,
                    null,
                    truckLicensePlate,
                    dateTimeNow,
                    null
                );

                dBContext.Trucks.Add(newTruck);

                await dBContext.SaveChangesAsync();

                return dBContext.Shipments
                    .Include(x => x.ShipmentDataHistory)
                    .Include(x => x.ShipmentBoxes)
                    .Include(x => x.TruckData)
                    .FirstOrDefault(x => x.Id == shipmentId);

            }
            return null;
        }
    }
}
