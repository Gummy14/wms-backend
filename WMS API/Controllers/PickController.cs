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
