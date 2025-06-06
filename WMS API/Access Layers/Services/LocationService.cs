using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IItemRepository _itemRepository;
        private ControllerFunctions controllerFunctions;

        public LocationService(
            ILocationRepository locationRepository,
            IItemRepository itemRepository
        )
        {
            _locationRepository = locationRepository;
            _itemRepository = itemRepository;
            controllerFunctions = new ControllerFunctions();
        }
        
        public async Task<List<Location>> GetAllLocationsMostRecentDataAsync()
        {
            var rootLocations = await _locationRepository.GetAllRootLocations();
            return rootLocations;
        }

        public async Task<Location> GetLocationByIdAsync(Guid locationId)
        {
            var result = await _locationRepository.GetLocationByIdAsync(locationId);
            return result;
        }

        public async Task<List<LocationData>> GetLocationHistoryByIdAsync(Guid locationId)
        {
            var result = await _locationRepository.GetLocationHistoryByIdAsync(locationId);
            return result;
        }

        public async Task<Location> GetPutawayLocationAsync()
        {
            var result = await _locationRepository.GetPutawayLocationAsync();
            return result;
        }

        public async Task RegisterLocationAsync(UnregisteredLocation locationToRegister)
        {
            Guid locationId = Guid.NewGuid();

            LocationData newLocationData = new LocationData(
                DateTime.Now,
                locationToRegister.XCoordinate,
                locationToRegister.YCoordinate,
                locationToRegister.ZCoordinate,
                locationToRegister.Description,
                locationToRegister.LengthInCentimeters,
                locationToRegister.WidthInCentimeters,
                locationToRegister.HeightInCentimeters,
                locationToRegister.WeightOrMaxWeightInKilograms,
                "Location Registered",
                locationId,
                Guid.NewGuid(),
                null,
                null
            );

            Location newLocation = new Location(
                locationId,
                new List<LocationData>() { newLocationData },
                null,
                null,
                null,
                null
            );

            await _locationRepository.AddLocationAsync(newLocation);
            controllerFunctions.printQrCode(locationToRegister.ObjectType + "-" + locationId);
        }

        public async Task PutawayItemIntoLocationAsync(Guid itemId, Guid locationId)
        {
            var itemDataToUpdate = await _itemRepository.GetItemDataByIdAsync(itemId);
            var locationDataToUpdate = await _locationRepository.GetLocationDataByIdAsync(locationId);

            if (itemDataToUpdate != null && locationDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemDataEventId = Guid.NewGuid();
                itemDataToUpdate.NextEventId = newItemDataEventId;

                ItemData newItemData = new ItemData(
                    dateTimeNow,
                    itemDataToUpdate.Name,
                    itemDataToUpdate.Description,
                    itemDataToUpdate.LengthInCentimeters,
                    itemDataToUpdate.WidthInCentimeters,
                    itemDataToUpdate.HeightInCentimeters,
                    itemDataToUpdate.WeightInKilograms,
                    "Item Putaway Into Location",
                    itemDataToUpdate.ItemType,
                    itemDataToUpdate.ItemId,
                    locationDataToUpdate.LocationId,
                    itemDataToUpdate.OrderId,
                    itemDataToUpdate.BoxId,
                    newItemDataEventId,
                    null,
                    itemDataToUpdate.EventId
                );

                await _itemRepository.AddItemDataAsync(newItemData);
            }
        }
    }
}
