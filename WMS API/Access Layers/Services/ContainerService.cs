using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Containers;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services
{
    public class ContainerService : IContainerService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IContainerRepository _containerRepository;
        private readonly IOrderRepository _orderRepository;
        private ControllerFunctions controllerFunctions;

        public ContainerService(
            IItemRepository itemRepository,
            ILocationRepository locationRepository,
            IContainerRepository containerRepository,
            IOrderRepository orderRepository
        )
        {
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
            _containerRepository = containerRepository;
            _orderRepository = orderRepository;
            controllerFunctions = new ControllerFunctions();
        }

        public async Task<List<Container>> GetAllContainersMostRecentDataAsync()
        {
            var result = await _containerRepository.GetAllContainersMostRecentDataAsync();
            return result;
        }

        public async Task<Container> GetContainerByIdAsync(Guid containerId)
        {
            var result = await _containerRepository.GetContainerByIdAsync(containerId);
            return result;
        }

        public async Task<List<ContainerData>> GetContainerHistoryByIdAsync(Guid containerId)
        {
            var result = await _containerRepository.GetContainerHistoryByIdAsync(containerId);
            return result;
        }

        public async Task RegisterContainerAsync(UnregisteredObject objectToRegister)
        {
            Guid containerId = Guid.NewGuid();

            ContainerData newContainerData = new ContainerData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                "Container Registered",
                containerId,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Container newContainer = new Container(
                containerId,
                new List<ContainerData>() { newContainerData },
                null
            );

            await _containerRepository.AddContainerAsync(newContainer);
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + containerId);
        }
        
        public async Task<Order> PickItemIntoContainerAsync(Guid itemId, Guid containerId)
        {
            var itemDataToUpdate = await _itemRepository.GetItemDataByIdAsync(itemId);
            var locationDataToUpdate = await _locationRepository.GetLocationDataByIdAsync((Guid)itemDataToUpdate.LocationId);
            var containerDataToUpdate = await _containerRepository.GetContainerDataByIdAsync(containerId);

            if (itemDataToUpdate == null || locationDataToUpdate == null || containerDataToUpdate == null)
                return null;

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
                "Item Picked Into Container",
                itemDataToUpdate.ItemType,
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
                locationDataToUpdate.Zone,
                locationDataToUpdate.Shelf,
                locationDataToUpdate.Row,
                locationDataToUpdate.Column,
                locationDataToUpdate.Description,
                locationDataToUpdate.LengthInCentimeters,
                locationDataToUpdate.WidthInCentimeters,
                locationDataToUpdate.HeightInCentimeters,
                locationDataToUpdate.MaxWeightInKilograms,
                "Location Emptied",
                locationDataToUpdate.LocationId,
                null,
                newLocationDataEventId,
                null,
                locationDataToUpdate.EventId
            );
            await _itemRepository.AddItemDataAsync(newItemData);
            await _locationRepository.AddLocationDataAsync(newLocationData);
            return await _orderRepository.GetOrderByIdAsync((Guid)containerDataToUpdate.OrderId);
        }
    }
}
