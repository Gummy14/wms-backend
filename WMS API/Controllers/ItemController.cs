using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Boxes;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
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

                dBContext.ItemData.Add(newItemData);
                dBContext.LocationData.Add(newLocationData);

                await dBContext.SaveChangesAsync();

                return newItemData;
            }
            return null;
        }

        [HttpPost("PickItem/{itemId}/{containerId}")]
        public async Task<Order> PickItem(Guid itemId, Guid containerId)
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

                dBContext.ItemData.Add(newItemData);
                dBContext.LocationData.Add(newLocationData);

                await dBContext.SaveChangesAsync();

                return dBContext.Orders
                    .Include(x => x.OrderDataHistory)
                    .Include(x => x.OrderItems)
                    .Include(x => x.Address)
                    .Include(x => x.ContainerUsedToPickOrder)
                    .FirstOrDefault(x => x.Id == containerDataToUpdate.OrderId);
            }
            return null;
        }
        
        [HttpPost("PackItem/{itemId}/{boxId}")]
        public async Task<Order> PackItem(Guid itemId, Guid boxId)
        {
            var itemDataToUpdate = dBContext.ItemData.FirstOrDefault(x => x.NextEventId == null && x.ItemId == itemId);
            var boxDataToUpdate = dBContext.BoxData.FirstOrDefault(x => x.NextEventId == null && x.BoxId == boxId);

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
                dBContext.ItemData.Add(newItemData);

                await dBContext.SaveChangesAsync();

                return dBContext.Orders
                    .Include(x => x.OrderDataHistory)
                    .Include(x => x.OrderItems)
                    .Include(x => x.Address)
                    .Include(x => x.ContainerUsedToPickOrder)
                    .FirstOrDefault(x => x.Id == boxDataToUpdate.OrderId);
            }

            return null;
        }
    }
}
