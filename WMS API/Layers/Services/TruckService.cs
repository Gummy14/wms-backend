using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Trucks;

namespace WMS_API.Layers.Services
{
    public class TruckService : ITruckService
    {
        private readonly ITruckRepository _truckRepository;
        private readonly IBoxRepository _boxRepository;

        public TruckService(
            ITruckRepository truckRepository, 
            IBoxRepository boxRepository
        )
        {
            _truckRepository = truckRepository;
            _boxRepository = boxRepository;
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
        
        public async Task AddBoxToTruckAsync(Guid boxId, Guid truckId)
        {
            var boxDataToUpdate = await _boxRepository.GetBoxByIdAsync(boxId);
            var truckData = await _truckRepository.GetTruckByIdAsync(truckId);

            if (boxDataToUpdate != null && truckData != null)
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
                    boxDataToUpdate.ShipmentId,
                    truckData.Id,
                    boxDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                    boxDataToUpdate.EventId
                );

                await _boxRepository.AddBoxDataAsync(newBoxData);
            }
        }
    }
}
