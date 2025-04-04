using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
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
            return dBContext.Locations.FirstOrDefault(x => x.NextEventId == Guid.Empty);
        }

        //POST
        [HttpPost("UpdateItemPutInLocation/{itemId}/{locationId}")]
        public async Task<Item> UpdateItemPutInLocation(Guid itemId, Guid locationId)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == itemId && x.NextEventId == Guid.Empty);
            var locationToUpdate = dBContext.Locations.FirstOrDefault(x => x.Id == locationId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null && locationToUpdate != null)
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
                    Constants.ITEM_PUTAWAY_INTO_LOCATION_COMPLETE,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    locationToUpdate.Id,
                    locationToUpdate.Name,
                    itemToUpdate.ContainerId,
                    itemToUpdate.ContainerName,
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
                    Constants.LOCATION_OCCUPIED,
                    locationToUpdate.EventId,
                    Guid.Empty,
                    itemToUpdate.Id,
                    itemToUpdate.Name
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
