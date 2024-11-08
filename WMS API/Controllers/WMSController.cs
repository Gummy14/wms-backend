using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WMS_API.DbContexts;
using WMS_API.Migrations;
using WMS_API.Models;
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
            return dBContext.Items.FirstOrDefault(x => x.ItemId == itemId && x.NextItemEventId == Guid.Empty);
        }

        [HttpGet("GetAllOrders")]
        public IList<Order> GetAllOrders()
        {
            return dBContext.Orders.Include(x => x.Items.Where(y => y.NextItemEventId == Guid.Empty)).ToList();
        }

        [HttpGet("GetPutawayLocation")]
        public Container GetPutawayLocation()
        {
            return dBContext.Containers.Where(x => x.ItemId == Guid.Empty && x.NextContainerEventId == Guid.Empty).FirstOrDefault();
        }

        [HttpGet("GetItemContainerRelationship/{genericId}")]
        public ItemContainer GetItemContainerRelationship(Guid genericId)
        {
            ItemContainer itemContainer = new ItemContainer();
            var container = dBContext.Containers
                .FirstOrDefault(
                x => x.ContainerId == genericId &&
                x.NextContainerEventId == Guid.Empty);

            if (container != null)
            {
                var item = dBContext.Items
                    .FirstOrDefault(
                    x => x.ItemId == container.ItemId &&
                    x.NextItemEventId == Guid.Empty);

                itemContainer.Item = item;
                itemContainer.Container = container;

                return itemContainer;
            }
            else
            {
                var item = dBContext.Items
                    .FirstOrDefault(
                    x => x.ItemId == genericId &&
                    x.NextItemEventId == Guid.Empty);

                var newContainer = dBContext.Containers
                    .FirstOrDefault(
                    x => x.ItemId == item.ItemId &&
                    x.NextContainerEventId == Guid.Empty);

                itemContainer.Item = item;
                itemContainer.Container = newContainer;

                return itemContainer;
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
                null,
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
                Guid.Empty,
                DateTime.Now, 
                2, 
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
            DateTime dateTimeNow = DateTime.Now;
            var containerToPutawayItemIn = dBContext.Containers.FirstOrDefault(x => x.ContainerEventId == container.ContainerEventId);

            if (containerToPutawayItemIn != null)
            {
                containerToPutawayItemIn.NextContainerEventId = containerEventId;

                Container newContainer = new Container(
                    containerEventId, 
                    containerToPutawayItemIn.ContainerId, 
                    containerToPutawayItemIn.Name,
                    container.ItemId,
                    dateTimeNow, 
                    3, 
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
            Guid orderId = Guid.NewGuid();
            var itemsToUpdateNextEventIdOn = dBContext.Items.Where(x => itemsInOrder.Contains(x));
            await itemsToUpdateNextEventIdOn.ForEachAsync(x => x.NextItemEventId = Guid.NewGuid());

            DateTime dateTimeNow = DateTime.Now;
            foreach (Item item in itemsInOrder) {
                item.OrderId = orderId;
                item.EventDateTime = dateTimeNow;
                item.EventType = 3;
                item.PreviousItemEventId = item.ItemEventId;
                item.ItemEventId = itemsToUpdateNextEventIdOn.FirstOrDefault(x => x.ItemEventId == item.ItemEventId).NextItemEventId;
            }

            Order order = new Order(Guid.NewGuid(), orderId, itemsInOrder, dateTimeNow, 7, Guid.Empty, Guid.Empty);

            dBContext.Orders.Add(order);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PickItem")]
        public async Task<StatusCodeResult> PickItem(Container container)
        {
            Guid pickBeforeContainerEventId = Guid.NewGuid();
            Guid pickAfterContainerEventId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            var containerToPickItemFrom = dBContext.Containers.FirstOrDefault(x => x.ContainerEventId == container.ContainerEventId);

            if (containerToPickItemFrom != null)
            {
                containerToPickItemFrom.NextContainerEventId = pickBeforeContainerEventId;

                Container pickBeforeContainer = new Container(
                    pickBeforeContainerEventId,
                    containerToPickItemFrom.ContainerId,
                    containerToPickItemFrom.Name,
                    containerToPickItemFrom.ItemId,
                    dateTimeNow,
                    5,
                    containerToPickItemFrom.ContainerEventId,
                    pickAfterContainerEventId
                );
                Container pickAfterContainer = new Container(
                    pickAfterContainerEventId,
                    containerToPickItemFrom.ContainerId,
                    containerToPickItemFrom.Name,
                    Guid.Empty,
                    dateTimeNow,
                    6,
                    pickBeforeContainerEventId,
                    Guid.Empty
                );
                dBContext.Entry(pickBeforeContainer).State = EntityState.Added;
                dBContext.Entry(pickAfterContainer).State = EntityState.Added;
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
