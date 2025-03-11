using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Containers;
using Container = WMS_API.Models.Containers.Container;

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
        public async Task<Item> UpdateItemSelectForPick(Guid itemId)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == itemId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newEventId;
                Item newItem = new Item(
                    newEventId,
                    itemToUpdate.Id,
                    itemToUpdate.Name,
                    itemToUpdate.Description,
                    DateTime.Now,
                    Constants.ITEM_SELECTED_FOR_PICK_PICK_IN_PROGRESS,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    itemToUpdate.LocationId,
                    itemToUpdate.LocationName,
                    itemToUpdate.ContainerId,
                    itemToUpdate.ContainerName,
                    itemToUpdate.OrderId,
                    itemToUpdate.OrderName
                );

                dBContext.Entry(newItem).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }

        [HttpPost("UpdateItemPick/{itemId}/{containerId}")]
        public async Task<Item> UpdateItemPick(Guid itemId, Guid containerId)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == itemId && x.NextEventId == Guid.Empty);
            var locationToUpdate = dBContext.Locations.FirstOrDefault(x => x.Id == itemToUpdate.LocationId && x.NextEventId == Guid.Empty);
            var containerToUpdate = dBContext.Containers.FirstOrDefault(x => x.Id == containerId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null && locationToUpdate != null && containerToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newItemEventId;
                Item newItem = new Item(
                    newItemEventId,
                    itemToUpdate.Id,
                    itemToUpdate.Name,
                    itemToUpdate.Description,
                    dateTimeNow,
                    Constants.ITEM_PICKED_INTO_CONTAINER,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    Guid.Empty,
                    "",
                    containerToUpdate.Id,
                    containerToUpdate.Name,
                    itemToUpdate.OrderId,
                    itemToUpdate.OrderName
                );

                Guid newLocationEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newLocationEventId;
                Location newLocation = new Location(
                    newLocationEventId,
                    locationToUpdate.Id,
                    locationToUpdate.Name,
                    locationToUpdate.Description,
                    dateTimeNow,
                    Constants.LOCATION_UNOCCUPIED,
                    locationToUpdate.EventId,
                    Guid.Empty,
                    Guid.Empty,
                    ""
                );

                Guid newContainerEventId = Guid.NewGuid();
                containerToUpdate.NextEventId = newContainerEventId;
                Container newContainer = new Container(
                    newContainerEventId,
                    containerToUpdate.Id,
                    containerToUpdate.Name,
                    containerToUpdate.Description,
                    dateTimeNow,
                    Constants.CONTAINER_IN_USE,
                    containerToUpdate.EventId,
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
