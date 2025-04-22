using Microsoft.EntityFrameworkCore;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Shipments;
using WMS_API.Models.Trucks;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly ITruckRepository _truckRepository;
        private readonly IAddressRepository _addressRepository;
        private ControllerFunctions controllerFunctions;

        public ShipmentService(
            IShipmentRepository shipmentRepository,
            IBoxRepository boxRepository,
            ITruckRepository truckRepository,
            IAddressRepository addressRepository
        )
        {
            _shipmentRepository = shipmentRepository;
            _boxRepository = boxRepository;
            _truckRepository = truckRepository;
            _addressRepository = addressRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<Shipment>> GetAllShipmentsAsync()
        {
            var result = await _shipmentRepository.GetAllShipmentsAsync();
            return result;
        }

        public async Task<Shipment> GetShipmentByIdAsync(Guid shipmentId)
        {
            var result = await _shipmentRepository.GetShipmentByIdAsync(shipmentId);
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
        
        public async Task AddBoxToShipmentAsync(Guid boxId)
        {
            var boxDataToUpdate = await _boxRepository.GetBoxDataByIdAsync(boxId);
            var shipmentData = await _shipmentRepository.GetNextShipmentAsync();
            var addressToPrint = await _addressRepository.GetAddressByOrderIdAsync((Guid)boxDataToUpdate.OrderId);

            if (boxDataToUpdate != null && shipmentData != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newBoxDataEventId = Guid.NewGuid();
                boxDataToUpdate.NextEventId = newBoxDataEventId;

                BoxData newBoxData = new BoxData(
                    dateTimeNow,
                    boxDataToUpdate.Name,
                    boxDataToUpdate.Description,
                    boxDataToUpdate.LengthInCentimeters,
                    boxDataToUpdate.WidthInCentimeters,
                    boxDataToUpdate.HeightInCentimeters,
                    boxDataToUpdate.IsSealed,
                    boxDataToUpdate.BoxId,
                    shipmentData.ShipmentId,
                    boxDataToUpdate.TruckId,
                    boxDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                    boxDataToUpdate.EventId
                );

                await _boxRepository.AddBoxDataAsync(newBoxData);
                controllerFunctions.printShippingLabel(addressToPrint);
            }
        }

        public async Task AddTruckToShipmentAsync(Guid shipmentId, string truckLicensePlate)
        {
            var shipmentToUpdate = await _shipmentRepository.GetShipmentDataByIdAsync(shipmentId);

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
