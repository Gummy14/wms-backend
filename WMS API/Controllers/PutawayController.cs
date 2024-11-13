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
    public class PutawayController : ControllerBase
    {
        private MyDbContext dBContext;

        public PutawayController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetPutawayLocation")]
        public Container GetPutawayLocation()
        {
            return dBContext.Containers.Where(x => x.ItemId == Guid.Empty && x.NextContainerEventId == Guid.Empty).FirstOrDefault();
        }

        [HttpPost("PutawayItem")]
        public async Task<StatusCodeResult> PutawayItem(Container container)
        {
            Guid containerEventId = Guid.NewGuid();
            Guid itemEventId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            var containerToPutawayItemIn = dBContext.Containers.FirstOrDefault(x => x.ContainerEventId == container.ContainerEventId);
            var itemToPutaway = dBContext.Items.FirstOrDefault(x => x.ItemId == container.ItemId && x.NextItemEventId == Guid.Empty);

            if (containerToPutawayItemIn != null)
            {
                containerToPutawayItemIn.NextContainerEventId = containerEventId;
                itemToPutaway.NextItemEventId = itemEventId;

                Item newItem = new Item(
                    itemEventId,
                    itemToPutaway.ItemId,
                    itemToPutaway.Name,
                    itemToPutaway.Description,
                    containerToPutawayItemIn.ContainerId,
                    itemToPutaway.OrderId,
                    dateTimeNow,
                    Constants.ITEM_PUTAWAY_INTO_CONTAINER_COMPLETE,
                    itemToPutaway.ItemEventId,
                    Guid.Empty
                );

                Container newContainer = new Container(
                    containerEventId,
                    containerToPutawayItemIn.ContainerId,
                    containerToPutawayItemIn.Name,
                    container.ItemId,
                    dateTimeNow,
                    Constants.ITEM_PUTAWAY_INTO_CONTAINER_COMPLETE,
                    containerToPutawayItemIn.ContainerEventId,
                    Guid.Empty
                );
                dBContext.Entry(newItem).State = EntityState.Added;
                dBContext.Entry(newContainer).State = EntityState.Added;
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
