using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private ControllerFunctions controllerFunctions;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
            controllerFunctions = new ControllerFunctions();
        }
        public async Task<List<LocationData>> GetAllLocationsAsync()
        {
            var result = await _locationRepository.GetAllLocationsAsync();
            return result;
        }

        public async Task<LocationData> GetLocationByIdAsync(Guid locationId)
        {
            var result = await _locationRepository.GetLocationByIdAsync(locationId);
            return result;
        }

        public async Task<List<LocationData>> GetLocationHistoryAsync(Guid locationId)
        {
            var result = await _locationRepository.GetLocationHistoryAsync(locationId);
            return result;
        }

        public async Task<LocationData> GetPutawayLocationAsync()
        {
            var result = await _locationRepository.GetPutawayLocationAsync();
            return result;
        }

        public async Task RegisterLocationAsync(UnregisteredObject objectToRegister)
        {
            Guid locationId = Guid.NewGuid();

            LocationData newLocationData = new LocationData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                objectToRegister.WeightOrMaxWeightInKilograms,
                locationId,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Location newLocation = new Location(
                locationId,
                new List<LocationData>() { newLocationData },
                null
            );

            await _locationRepository.AddLocationAsync(newLocation);
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + locationId);
        }
    }
}
