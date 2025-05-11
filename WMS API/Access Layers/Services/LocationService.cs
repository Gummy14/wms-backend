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
            var result = await _locationRepository.GetAllLocationsMostRecentDataAsync();
            return result;
        }

        public async Task<Location> GetLocationByIdAsync(Guid locationId)
        {
            var result = await _locationRepository.GetLocationByIdAsync(locationId);
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
                "Location Registered",
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

        public async Task PutawayItemIntoLocationAsync(Guid itemId, Guid locationId)
        {
            var itemDataToUpdate = await _itemRepository.GetItemDataByIdAsync(itemId);
            var locationDataToUpdate = await _locationRepository.GetLocationDataByIdAsync(locationId);

            if (itemDataToUpdate != null && locationDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemDataEventId = Guid.NewGuid();
                itemDataToUpdate.NextEventId = newItemDataEventId;

                Guid newLocationDataEventId = Guid.NewGuid();
                locationDataToUpdate.NextEventId = newLocationDataEventId;

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
                    itemDataToUpdate.ContainerId,
                    itemDataToUpdate.OrderId,
                    itemDataToUpdate.BoxId,
                    newItemDataEventId,
                    null,
                    itemDataToUpdate.EventId
                );

                LocationData newLocationData = new LocationData(
                    dateTimeNow,
                    locationDataToUpdate.Name,
                    locationDataToUpdate.Description,
                    locationDataToUpdate.LengthInCentimeters,
                    locationDataToUpdate.WidthInCentimeters,
                    locationDataToUpdate.HeightInCentimeters,
                    locationDataToUpdate.MaxWeightInKilograms,
                    "Item Putaway Into Location",
                    locationDataToUpdate.LocationId,
                    itemDataToUpdate.ItemId,
                    newLocationDataEventId,
                    null,
                    locationDataToUpdate.EventId
                );

                await _itemRepository.AddItemDataAsync(newItemData);
                await _locationRepository.AddLocationDataAsync(newLocationData);
            }
        }
    }
}
