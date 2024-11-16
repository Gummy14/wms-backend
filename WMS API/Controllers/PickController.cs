using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Containers;
using WMS_API.Models.Items;

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

        [HttpPost("PickItem")]
        public async Task<Item> PickItem(Item item)
        {
            var itemToPick = dBContext.Items.FirstOrDefault(x => x.ItemId == item.ItemId && x.NextItemEventId == Guid.Empty);

            if (itemToPick != null)
            {
                Guid pickBeforeItemEventId = Guid.NewGuid();
                Guid pickAfterItemEventId = Guid.NewGuid();
                DateTime dateTimeNow = DateTime.Now;

                itemToPick.NextItemEventId = pickBeforeItemEventId;

                Item pickBeforeItem = new Item(
                    pickBeforeItemEventId,
                    itemToPick.ItemId,
                    itemToPick.Name,
                    itemToPick.Description,
                    itemToPick.ContainerId,
                    itemToPick.OrderId,
                    dateTimeNow,
                    Constants.ITEM_PICKED_FROM_CONTAINER_BEFORE,
                    itemToPick.ItemEventId,
                    pickAfterItemEventId
                );
                Item pickAfterItem = new Item(
                    pickAfterItemEventId,
                    itemToPick.ItemId,
                    itemToPick.Name,
                    itemToPick.Description,
                    item.ContainerId,
                    itemToPick.OrderId,
                    dateTimeNow,
                    Constants.ITEM_PICKED_FROM_CONTAINER_AFTER,
                    pickBeforeItemEventId,
                    Guid.Empty
                );

                dBContext.Entry(pickBeforeItem).State = EntityState.Added;
                dBContext.Entry(pickAfterItem).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return pickAfterItem;
            };

            return null;
        }
    }
}
