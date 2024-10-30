using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WMS_API.DbContexts;
using WMS_API.Migrations;
using WMS_API.Models.Containers;
using WMS_API.Models.Events;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WMSController : ControllerBase
    {
        private MyDbContext dBContext;

        public WMSController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetAllItems")]
        public IList<Item> GetAllItems()
        {
            return dBContext.Items.Where(x => x.NextItemEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(Guid itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.ItemId == itemId && x.NextItemEventId == null);
        }

        [HttpGet("GetAllOrders")]
        public IList<Order> GetAllOrders()
        {
            return dBContext.Orders.Include(x => x.Items).ToList();
        }

        [HttpGet("GetPutawayLocation")]
        public Container GetPutawayLocation()
        {
            return dBContext.Containers.Where(x => x.Item.ItemId == null && x.NextContainerEventId == Guid.Empty).FirstOrDefault();
        }

        [HttpGet("GetItemContainerRelationship/{genericId}")]
        public Container GetItemContainerRelationship(Guid genericId)
        {
            var container = dBContext.Containers
                .Include(x => x.Item)
                .FirstOrDefault(
                x => x.ContainerId == genericId && 
                x.NextContainerEventId == Guid.Empty);

            if(container != null) {
                return container;
            } else {
                return dBContext.Containers
                    .Include(x => x.Item)
                    .FirstOrDefault(
                    x => x.Item.ItemId == genericId && 
                    x.NextContainerEventId == Guid.Empty);
            }
        }

        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(ItemToRegister itemToRegister)
        {
            Item item = new Item(
                Guid.NewGuid(),
                Guid.NewGuid(),
                itemToRegister.Name, 
                itemToRegister.Description, 
                DateTime.Now,
                1,
                Guid.Empty,
                Guid.Empty
            );

            dBContext.Items.Add(item);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(ContainerToRegister containerToRegister)
        {
            Container container = new Container(
                Guid.NewGuid(), 
                Guid.NewGuid(), 
                containerToRegister.Name,
                DateTime.Now, 
                1, 
                Guid.Empty, 
                Guid.Empty
            );

            dBContext.Containers.Add(container);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PutawayItem")]
        public async Task<StatusCodeResult> PutawayItem(Container container)
        {
            Guid containerEventId = Guid.NewGuid();
            var containerToPutawayItemIn = dBContext.Containers.FirstOrDefault(x => x.ContainerEventId == container.ContainerEventId);

            if (containerToPutawayItemIn != null)
            {
                containerToPutawayItemIn.NextContainerEventId = containerEventId;
                Container newContainer = new Container(
                    containerEventId, 
                    containerToPutawayItemIn.ContainerId, 
                    containerToPutawayItemIn.Name, 
                    container.Item, 
                    DateTime.Now, 
                    2, 
                    containerToPutawayItemIn.ContainerEventId, 
                    Guid.Empty
                );
                dBContext.Entry(newContainer).State = EntityState.Added;
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("CreateOrder")]
        public async Task<StatusCodeResult> CreateOrder(List<Item> itemsInOrder)
        {
            var itemsToUpdateNextEventIdOn = dBContext.Items.Where(x => itemsInOrder.Contains(x));
            await itemsToUpdateNextEventIdOn.ForEachAsync(x => x.NextItemEventId = Guid.NewGuid());

            DateTime dateTimeNow = DateTime.Now;
            foreach (Item item in itemsInOrder) {
                item.EventDateTime = dateTimeNow;
                item.EventType = 3;
                item.PreviousItemEventId = item.ItemEventId;
                item.ItemEventId = itemsToUpdateNextEventIdOn.FirstOrDefault(x => x.ItemEventId == item.ItemEventId).NextItemEventId;
            }

            Order order = new Order(Guid.NewGuid(), itemsInOrder, dateTimeNow);

            dBContext.Orders.Add(order);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PickItem")]
        public async Task<StatusCodeResult> PickItem(Container container)
        {
            Guid containerEventId = Guid.NewGuid();
            var containerToPickItemFrom = dBContext.Containers.Include(x => x.Item).FirstOrDefault(x => x.ContainerEventId == container.ContainerEventId);

            if (containerToPickItemFrom != null)
            {
                containerToPickItemFrom.NextContainerEventId = containerEventId;
                Container newContainer = new Container(
                    containerEventId,
                    containerToPickItemFrom.ContainerId,
                    containerToPickItemFrom.Name,
                    null,
                    DateTime.Now,
                    4,
                    containerToPickItemFrom.ContainerEventId,
                    Guid.Empty
                );
                dBContext.Entry(newContainer).State = EntityState.Added;
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
