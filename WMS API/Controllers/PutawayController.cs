using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PutawayController : ControllerBase
    {
        private MyDbContext dBContext;

        public PutawayController(MyDbContext context)
        {
            dBContext = context;
        }

        //GET
        [HttpGet("GetPutawayLocation")]
        public WarehouseObject GetPutawayLocation()
        {
            return dBContext.WarehouseObjects.FirstOrDefault(x => 
                x.ObjectType == 1 && 
                x.ItemId == Guid.Empty && 
                x.NextEventId == Guid.Empty
            );
        }

        //POST
        [HttpPost("UpdateItemSelectForPutaway/{itemId}")]
        public async Task<WarehouseObject> UpdateItemSelectForPutaway(Guid itemId)
        {
            var itemToUpdate = dBContext.WarehouseObjects.FirstOrDefault(x => 
                x.ItemId == itemId && 
                x.ObjectType == 0 && 
                x.NextEventId == Guid.Empty
            );

            if (itemToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newEventId;
                WarehouseObject newItem = new WarehouseObject(
                    newEventId,
                    itemToUpdate.ObjectType,
                    itemToUpdate.ItemId,
                    itemToUpdate.ItemName,
                    itemToUpdate.ItemDescription,
                    itemToUpdate.LocationId,
                    itemToUpdate.LocationName,
                    itemToUpdate.LocationDescription,
                    itemToUpdate.ContainerId,
                    itemToUpdate.ContainerName,
                    itemToUpdate.ContainerDescription,
                    itemToUpdate.OrderId,
                    itemToUpdate.OrderName,
                    itemToUpdate.OrderDescription,
                    DateTime.Now,
                    Constants.ITEM_SELECTED_FOR_PUTAWAY_PUTAWAY_IN_PROGRESS,
                    itemToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newItem).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }

        [HttpPost("UpdateItemPutInLocation/{itemId}/{locationId}")]
        public async Task<WarehouseObject> UpdateItemPutInLocation(Guid itemId, Guid locationId)
        {
            var itemToUpdate = dBContext.WarehouseObjects.FirstOrDefault(x =>
                x.ItemId == itemId &&
                x.ObjectType == 0 &&
                x.NextEventId == Guid.Empty
            );

            var locationToUpdate = dBContext.WarehouseObjects.FirstOrDefault(x =>
                x.LocationId == locationId &&
                x.ObjectType == 1 &&
                x.NextEventId == Guid.Empty
            );

            if (itemToUpdate != null && locationToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newItemEventId;
                WarehouseObject newItem = new WarehouseObject(
                    newItemEventId,
                    itemToUpdate.ObjectType,
                    itemToUpdate.ItemId,
                    itemToUpdate.ItemName,
                    itemToUpdate.ItemDescription,
                    locationToUpdate.LocationId,
                    locationToUpdate.LocationName,
                    locationToUpdate.LocationDescription,
                    itemToUpdate.ContainerId,
                    itemToUpdate.ContainerName,
                    itemToUpdate.ContainerDescription,
                    itemToUpdate.OrderId,
                    itemToUpdate.OrderName,
                    itemToUpdate.OrderDescription,
                    DateTime.Now,
                    Constants.ITEM_PUTAWAY_INTO_LOCATION_COMPLETE,
                    itemToUpdate.EventId,
                    Guid.Empty
                );

                Guid newLocationEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newLocationEventId;
                WarehouseObject newLocation = new WarehouseObject(
                    newLocationEventId,
                    locationToUpdate.ObjectType,
                    itemToUpdate.ItemId,
                    itemToUpdate.ItemName,
                    itemToUpdate.ItemDescription,
                    locationToUpdate.LocationId,
                    locationToUpdate.LocationName,
                    locationToUpdate.LocationDescription,
                    locationToUpdate.ContainerId,
                    locationToUpdate.ContainerName,
                    locationToUpdate.ContainerDescription,
                    locationToUpdate.OrderId,
                    locationToUpdate.OrderName,
                    locationToUpdate.OrderDescription,
                    DateTime.Now,
                    Constants.LOCATION_OCCUPIED,
                    locationToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newItem).State = EntityState.Added;
                dBContext.Entry(newLocation).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }
    }
}
