using Microsoft.AspNetCore.Mvc;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IContainerRepository _containerRepository;
        private readonly IBoxRepository _boxRepository;
        private ControllerFunctions controllerFunctions;

        public ItemService(
            IItemRepository itemRepository, 
            ILocationRepository locationRepository, 
            IContainerRepository containerRepository,
            IBoxRepository boxRepository
        )
        {
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
            _containerRepository = containerRepository;
            _boxRepository = boxRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<ItemData>> GetAllItemsAsync()
        {
            var result = await _itemRepository.GetAllItemsAsync();
            return result;
        }

        public async Task<ItemData> GetItemByIdAsync(Guid itemId)
        {
            var result = await _itemRepository.GetItemByIdAsync(itemId);
            return result;
        }

        public async Task<List<ItemData>> GetItemHistoryAsync(Guid itemId)
        {
            var result = await _itemRepository.GetItemHistoryAsync(itemId);
            return result;
        }

        public async Task RegisterItemAsync(UnregisteredObject objectToRegister)
        {
            Guid itemId = Guid.NewGuid();

            ItemData newItemData = new ItemData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                objectToRegister.WeightOrMaxWeightInKilograms,
                itemId,
                null,
                null,
                null,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Item newItem = new Item(
                itemId,
                new List<ItemData>() { newItemData },
                null
            );

            await _itemRepository.AddItemAsync(newItem);
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + itemId);
        }
        
        public async Task PutawayItemAsync(Guid itemId, Guid locationId)
        {
            var itemDataToUpdate = await _itemRepository.GetItemByIdAsync(itemId);
            var locationDataToUpdate = await _locationRepository.GetLocationByIdAsync(locationId);

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

        public async Task PickItemAsync(Guid itemId, Guid containerId)
        {
            var itemDataToUpdate = await _itemRepository.GetItemByIdAsync(itemId);
            var locationDataToUpdate = await _locationRepository.GetLocationByIdAsync((Guid)itemDataToUpdate.LocationId);
            var containerDataToUpdate = await _containerRepository.GetContainerByIdAsync(containerId);

            if (itemDataToUpdate != null && locationDataToUpdate != null && containerDataToUpdate != null)
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
                    itemDataToUpdate.ItemId,
                    null,
                    containerDataToUpdate.ContainerId,
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
                    locationDataToUpdate.LocationId,
                    null,
                    newLocationDataEventId,
                    null,
                    locationDataToUpdate.EventId
                );
                await _itemRepository.AddItemDataAsync(newItemData);
                await _locationRepository.AddLocationDataAsync(newLocationData);
            }
        }

        public async Task PackItemAsync(Guid itemId, Guid boxId)
        {
            var itemDataToUpdate = await _itemRepository.GetItemByIdAsync(itemId);
            var boxDataToUpdate = await _boxRepository.GetBoxByIdAsync(boxId);

            if (itemDataToUpdate != null && boxDataToUpdate != null)
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
                    itemDataToUpdate.ItemId,
                    itemDataToUpdate.LocationId,
                    null,
                    itemDataToUpdate.OrderId,
                    boxDataToUpdate.BoxId,
                    newItemDataEventId,
                    null,
                    itemDataToUpdate.EventId
                );

                await _itemRepository.AddItemDataAsync(newItemData);
            }
        }
    }
}
