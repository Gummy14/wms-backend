using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Shipments;
using WMS_API.Models.Trucks;

namespace WMS_API.Layers.Services
{
    public class TruckService : ITruckService
    {
        private readonly ITruckRepository _truckRepository;
        private readonly IShipmentRepository _shipmentRepository;

        public TruckService(
            ITruckRepository truckRepository, 
            IShipmentRepository shipmentRepository
        )
        {
            _truckRepository = truckRepository;
            _shipmentRepository = shipmentRepository;
        }

        public async Task<List<Truck>> GetAllTrucksAsync()
        {
            var result = await _truckRepository.GetAllTrucksAsync();
            return result;
        }

        public async Task SetTruckDepartedAsync(Guid truckId)
        {
            await _truckRepository.UpdateTruckAsync(truckId);
        }
        
        public async Task AddShipmentToTruckAsync(Guid shipmentId, Guid truckId)
        {
            var shipmentDataToUpdate = await _shipmentRepository.GetShipmentDataByIdAsync(shipmentId);
            var truckData = await _truckRepository.GetTruckByIdAsync(truckId);

            if (shipmentDataToUpdate != null && truckData != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newShipmentDataEventId = Guid.NewGuid();
                shipmentDataToUpdate.NextEventId = newShipmentDataEventId;

                ShipmentData newShipmentData = new ShipmentData(
                    dateTimeNow,
                    shipmentDataToUpdate.Name,
                    shipmentDataToUpdate.Description,
                    "Shipment Added To Truck",
                    shipmentDataToUpdate.ShipmentId,
                    truckData.Id,
                    newShipmentDataEventId,
                    null,
                    shipmentDataToUpdate.EventId
                );

                await _shipmentRepository.AddShipmentDataAsync(newShipmentData);
            }
        }
    }
}
