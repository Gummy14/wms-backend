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
        public async Task<Item> PickItem(Container container)
        {
            Guid pickBeforeContainerEventId = Guid.NewGuid();
            Guid pickAfterContainerEventId = Guid.NewGuid();
            Guid pickBeforeItemEventId = Guid.NewGuid();
            Guid pickAfterItemEventId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            var containerToPickItemFrom = dBContext.Containers.FirstOrDefault(x => x.ContainerEventId == container.ContainerEventId);
            var itemToPick = dBContext.Items.FirstOrDefault(x => x.ItemId == container.ItemId && x.NextItemEventId == Guid.Empty);

            if (containerToPickItemFrom != null)
            {
                containerToPickItemFrom.NextContainerEventId = pickBeforeContainerEventId;
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
                    Guid.Empty,
                    itemToPick.OrderId,
                    dateTimeNow,
                    Constants.ITEM_PICKED_FROM_CONTAINER_AFTER,
                    pickBeforeItemEventId,
                    Guid.Empty
                );

                Container pickBeforeContainer = new Container(
                    pickBeforeContainerEventId,
                    containerToPickItemFrom.ContainerId,
                    containerToPickItemFrom.Name,
                    containerToPickItemFrom.ItemId,
                    dateTimeNow,
                    Constants.ITEM_PICKED_FROM_CONTAINER_BEFORE,
                    containerToPickItemFrom.ContainerEventId,
                    pickAfterContainerEventId
                );
                Container pickAfterContainer = new Container(
                    pickAfterContainerEventId,
                    containerToPickItemFrom.ContainerId,
                    containerToPickItemFrom.Name,
                    Guid.Empty,
                    dateTimeNow,
                    Constants.ITEM_PICKED_FROM_CONTAINER_AFTER,
                    pickBeforeContainerEventId,
                    Guid.Empty
                );
                dBContext.Entry(pickBeforeItem).State = EntityState.Added;
                dBContext.Entry(pickAfterItem).State = EntityState.Added;
                dBContext.Entry(pickBeforeContainer).State = EntityState.Added;
                dBContext.Entry(pickAfterContainer).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return pickAfterItem;
            };

            return null;
        }
    }
}
