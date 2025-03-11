using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PickController : ControllerBase
    {
        private MyDbContext dBContext;

        public PickController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpPost("UpdateItemSelectForPick/{itemId}")]
        public async Task<WarehouseObject> UpdateItemSelectForPick(Guid itemId)
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
                    Constants.ITEM_PUTAWAY_INTO_LOCATION_COMPLETE,
                    itemToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newItem).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }

        [HttpPost("UpdateItemPick/{itemId}/{containerId}")]
        public async Task<WarehouseObject> UpdateItemPick(Guid itemId, Guid containerId)
        {
            var itemToUpdate = dBContext.WarehouseObjects.FirstOrDefault(x =>
                x.ItemId == itemId &&
                x.ObjectType == 0 &&
                x.NextEventId == Guid.Empty
            );

            var locationToUpdate = dBContext.WarehouseObjects.FirstOrDefault(x =>
                x.LocationId == itemToUpdate.LocationId &&
                x.ObjectType == 1 &&
                x.NextEventId == Guid.Empty
            );

            var containerToUpdate = dBContext.WarehouseObjects.FirstOrDefault(x =>
                x.ContainerId == containerId &&
                x.ObjectType == 2 &&
                x.NextEventId == Guid.Empty
            );

            if (itemToUpdate != null && locationToUpdate != null && containerToUpdate != null)
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
                    Guid.Empty,
                    "",
                    "",
                    containerToUpdate.ContainerId,
                    containerToUpdate.ContainerName,
                    containerToUpdate.ContainerDescription,
                    itemToUpdate.OrderId,
                    itemToUpdate.OrderName,
                    itemToUpdate.OrderDescription,
                    DateTime.Now,
                    Constants.ITEM_PICKED_INTO_CONTAINER,
                    itemToUpdate.EventId,
                    Guid.Empty
                );

                Guid newLocationEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newLocationEventId;
                WarehouseObject newLocation = new WarehouseObject(
                    newLocationEventId,
                    locationToUpdate.ObjectType,
                    Guid.Empty,
                    "",
                    "",
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
                    Constants.LOCATION_UNOCCUPIED,
                    locationToUpdate.EventId,
                    Guid.Empty
                );

                Guid newContainerEventId = Guid.NewGuid();
                containerToUpdate.NextEventId = newContainerEventId;
                WarehouseObject newContainer = new WarehouseObject(
                    newContainerEventId,
                    locationToUpdate.ObjectType,
                    containerToUpdate.ItemId,
                    containerToUpdate.ItemName,
                    containerToUpdate.ItemDescription,
                    containerToUpdate.LocationId,
                    containerToUpdate.LocationName,
                    containerToUpdate.LocationDescription,
                    containerToUpdate.ContainerId,
                    containerToUpdate.ContainerName,
                    containerToUpdate.ContainerDescription,
                    containerToUpdate.OrderId,
                    containerToUpdate.OrderName,
                    containerToUpdate.OrderDescription,
                    DateTime.Now,
                    Constants.CONTAINER_IN_USE,
                    locationToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newItem).State = EntityState.Added;
                dBContext.Entry(newLocation).State = EntityState.Added;
                dBContext.Entry(newContainer).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }
    }
}
