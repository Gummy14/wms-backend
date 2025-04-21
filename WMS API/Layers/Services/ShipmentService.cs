using Microsoft.EntityFrameworkCore;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Shipments;
using WMS_API.Models.Trucks;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly ITruckRepository _truckRepository;
        private ControllerFunctions controllerFunctions;

        public ShipmentService(IShipmentRepository shipmentRepository, ITruckRepository truckRepository)
        {
            _shipmentRepository = shipmentRepository;
            _truckRepository = truckRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<ShipmentData>> GetAllShipmentsAsync()
        {
            var result = await _shipmentRepository.GetAllShipmentsAsync();
            return result;
        }

        public async Task<ShipmentData> GetShipmentByIdAsync(Guid shipmentId)
        {
            var result = await _shipmentRepository.GetShipmentByIdAsync(shipmentId);
            return result;
        }

        public async Task<List<ShipmentData>> GetShipmentHistoryAsync(Guid shipmentId)
        {
            var result = await _shipmentRepository.GetShipmentHistoryAsync(shipmentId);
            return result;
        }

        public async Task RegisterShipmentAsync(UnregisteredObject objectToRegister)
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

            await _shipmentRepository.AddShipmentAsync(newShipment);
        }

        public async Task AddShipmentToTruckAsync(Guid shipmentId, string truckLicensePlate)
        {
            var shipmentToUpdate = await _shipmentRepository.GetShipmentByIdAsync(shipmentId);

            if (shipmentToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid truckId = Guid.NewGuid();

                Truck newTruck = new Truck(
                    truckId,
                    shipmentToUpdate.ShipmentId,
                    null,
                    truckLicensePlate,
                    dateTimeNow,
                    null
                );

                await _truckRepository.AddTruckAsync(newTruck);
            }
        }
    }
}
