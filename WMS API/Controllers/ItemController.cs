using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Boxes;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using ContainerData = WMS_API.Models.Containers.ContainerData;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private MyDbContext dBContext;

        public ItemController(MyDbContext context)
        {
            dBContext = context;
        }

        //GET
        [HttpGet("GetAllItems")]
        public IList<ItemData> GetAllItems()
        {
            return dBContext.ItemData.Where(x => x.NextEventId == null).ToList();
        }

        [HttpGet("GetItemById/{itemId}")]
        public ItemData GetItemById(Guid itemId)
        {
            return dBContext.ItemData.FirstOrDefault(x => x.NextEventId == null && x.ItemId == itemId);
        }

        [HttpGet("GetItemHistory/{itemId}")]
        public List<ItemData> GetItemHistory(Guid itemId)
        {
            return dBContext.ItemData.Where(x => x.ItemId == itemId).ToList();
        }

        //POST
        [HttpPost("PutawayItem/{itemId}/{locationId}")]
        public async Task<ItemData> PutawayItem(Guid itemId, Guid locationId)
        {
            var itemDataToUpdate = dBContext.ItemData.FirstOrDefault(x => x.NextEventId == null && x.ItemId == itemId);
            var locationDataToUpdate = dBContext.LocationData.FirstOrDefault(x => x.NextEventId == null && x.LocationId == locationId);

            if (itemDataToUpdate != null && locationDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemDataEventId = Guid.NewGuid();
                itemDataToUpdate.NextEventId = newItemDataEventId;

                ItemData newItemData = new ItemData(
                    dateTimeNow,
                    Constants.ITEM_PUTAWAY_INTO_LOCATION_COMPLETE,
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

                Guid newLocationDataEventId = Guid.NewGuid();
                locationDataToUpdate.NextEventId = newLocationDataEventId;

                LocationData newLocationData = new LocationData(
                    dateTimeNow,
                    Constants.LOCATION_OCCUPIED,
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

                dBContext.ItemData.Add(newItemData);
                dBContext.LocationData.Add(newLocationData);

                await dBContext.SaveChangesAsync();

                return newItemData;
            }
            return null;
        }

        [HttpPost("PickItem/{itemId}/{containerId}")]
        public async Task<ItemData> PickItem(Guid itemId, Guid containerId)
        {
            var itemDataToUpdate = dBContext.ItemData.FirstOrDefault(x => x.NextEventId == null && x.ItemId == itemId);
            var locationDataToUpdate = dBContext.LocationData.FirstOrDefault(x => x.NextEventId == null && x.ItemId == itemId);
            var containerDataToUpdate = dBContext.ContainerData.FirstOrDefault(x => x.NextEventId == null && x.ContainerId == containerId);

            if (itemDataToUpdate != null && locationDataToUpdate != null && containerDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemDataEventId = Guid.NewGuid();
                itemDataToUpdate.NextEventId = newItemDataEventId;

                ItemData newItemData = new ItemData(
                    dateTimeNow,
                    Constants.ITEM_PICKED_INTO_CONTAINER,
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

                Guid newLocationDataEventId = Guid.NewGuid();
                locationDataToUpdate.NextEventId = newLocationDataEventId;

                LocationData newLocationData = new LocationData(
                    dateTimeNow,
                    Constants.LOCATION_UNOCCUPIED,
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

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    Constants.CONTAINER_IN_USE,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    newContainerDataEventId,
                    null,
                    containerDataToUpdate.EventId
                );

                dBContext.ItemData.Add(newItemData);
                dBContext.LocationData.Add(newLocationData);
                dBContext.ContainerData.Add(newContainerData);

                await dBContext.SaveChangesAsync();

                return newItemData;
            }
            return null;
        }
        
        [HttpPost("PackItems/{containerId}/{boxId}")]
        public async Task<BoxData> PackItems(Guid containerId, Guid boxId)
        {
            var itemDataToUpdate = dBContext.ItemData.Where(x => x.NextEventId == null && x.ContainerId == containerId).ToList();
            var containerDataToUpdate = dBContext.ContainerData.FirstOrDefault(x => x.NextEventId == null && x.ContainerId == containerId);
            var boxDataToUpdate = dBContext.BoxData.FirstOrDefault(x => x.NextEventId == null && x.BoxId == boxId);

            if (boxDataToUpdate != null && containerDataToUpdate != null) 
            {
                var dateTimeNow = DateTime.Now;

                foreach (var itemDataEntryToUpdate in itemDataToUpdate)
                {
                    ItemData newItemData = new ItemData(
                        dateTimeNow,
                        Constants.ITEM_PACKED_IN_BOX,
                        itemDataEntryToUpdate.Name,
                        itemDataEntryToUpdate.Description,
                        itemDataEntryToUpdate.LengthInCentimeters,
                        itemDataEntryToUpdate.WidthInCentimeters,
                        itemDataEntryToUpdate.HeightInCentimeters,
                        itemDataEntryToUpdate.WeightInKilograms,
                        itemDataEntryToUpdate.ItemId,
                        itemDataEntryToUpdate.LocationId,
                        null,
                        itemDataEntryToUpdate.OrderId,
                        boxDataToUpdate.BoxId,
                        Guid.NewGuid(),
                        null,
                        itemDataEntryToUpdate.EventId
                    );
                    dBContext.ItemData.Add(newItemData);
                }

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    Constants.CONTAINER_NOT_IN_USE,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    Guid.NewGuid(),
                    null,
                    containerDataToUpdate.EventId
                );
                dBContext.ContainerData.Add(newContainerData);

                await dBContext.SaveChangesAsync();

                return boxDataToUpdate;
            }

            return null;
        }
    }
}
