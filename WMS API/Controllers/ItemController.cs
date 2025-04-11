using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Boxes;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.WarehouseObjects;
using ContainerData = WMS_API.Models.Containers.ContainerData;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;

        public ItemController(MyDbContext context)
        {
            dBContext = context;
            controllerFunctions = new ControllerFunctions();
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
        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(UnregisteredObject objectToRegister)
        {
            Guid itemId = Guid.NewGuid();

            ItemData newItemData = new ItemData(
                DateTime.Now,
                Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY,
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

            dBContext.Items.Add(newItem);
            await dBContext.SaveChangesAsync();
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + itemId);
            return StatusCode(200);
        }

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

                Guid newLocationDataEventId = Guid.NewGuid();
                locationDataToUpdate.NextEventId = newLocationDataEventId;

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

                Guid newLocationDataEventId = Guid.NewGuid();
                locationDataToUpdate.NextEventId = newLocationDataEventId;

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

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

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    Constants.CONTAINER_IN_USE,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    null,
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
                    Guid newItemDataEventId = Guid.NewGuid();
                    itemDataEntryToUpdate.NextEventId = newItemDataEventId;

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

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                Guid newBoxDataEventId = Guid.NewGuid();
                boxDataToUpdate.NextEventId = newBoxDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    Constants.CONTAINER_NOT_IN_USE,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    null,
                    Guid.NewGuid(),
                    null,
                    containerDataToUpdate.EventId
                );
                dBContext.ContainerData.Add(newContainerData);

                BoxData newBoxData = new BoxData(
                    dateTimeNow,
                    Constants.BOX_PACKED,
                    boxDataToUpdate.Name,
                    boxDataToUpdate.Description,
                    boxDataToUpdate.LengthInCentimeters,
                    boxDataToUpdate.WidthInCentimeters,
                    boxDataToUpdate.HeightInCentimeters,
                    boxDataToUpdate.BoxId,
                    Guid.NewGuid(),
                    null,
                    boxDataToUpdate.EventId
                );
                dBContext.BoxData.Add(newBoxData);

                await dBContext.SaveChangesAsync();

                //print shipping label
                //controllerFunctions.printShippingLabel(itemDataToUpdate.FirstOrDefault().OrderId)

                return boxDataToUpdate;
            }

            return null;
        }
    }
}
