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
        [HttpPost("UpdateItemSelectForPutaway/{itemId}")]
        public async Task<Item> UpdateItemSelectForPutaway(Guid itemId)
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
                    Constants.ITEM_SELECTED_FOR_PUTAWAY_PUTAWAY_IN_PROGRESS,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    itemToUpdate.LocationId,
                    itemToUpdate.LocationName,
                    itemToUpdate.ContainerId,
                    itemToUpdate.ContainerName,
                    itemToUpdate.OrderId,
                    itemToUpdate.OrderName,
                    itemToUpdate.BoxId,
                    itemToUpdate.BoxName
                );

                dBContext.Entry(newItem).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }
    }
}
