using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Trucks;

namespace WMS_API.Layers.Services
{
    public class TruckService : ITruckService
    {
        private readonly ITruckRepository _truckRepository;

        public TruckService(ITruckRepository truckRepository)
        {
            _truckRepository = truckRepository;
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
    }
}
