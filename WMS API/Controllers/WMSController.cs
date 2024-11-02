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

        [HttpGet("GetAllCurrentItems")]
        public IList<Item> GetAllCurrentItems()
        {
            return dBContext.Items.Where(x => x.NextItemEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(Guid itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.ItemId == itemId && x.NextItemEventId == Guid.Empty);
        }

        [HttpGet("GetAllCurrentOrdersWithOutItems")]
        public IList<OrderItems> GetAllCurrentOrdersWithOutItems()
        {
            List<OrderItems> orderItems = new List<OrderItems>();
            var currentOrders = dBContext.Orders.Where(x => x.NextOrderEventId == Guid.Empty).ToList();

            int loopIteration = 0;
            foreach (var order in currentOrders) {
                orderItems.Add(new OrderItems (order, null));
            }

            return orderItems;
        }

        [HttpGet("GetOrderItems/{orderId}")]
        public IList<Item> GetOrderItems(Guid orderId)
        {
            return dBContext.Items.Where(x => x.OrderId == orderId && x.NextItemEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetPutawayLocation")]
        public Container GetPutawayLocation()
        {
            return dBContext.Containers.Where(x => x.ItemEventId == Guid.Empty && x.NextContainerEventId == Guid.Empty).FirstOrDefault();
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
                x => x.ItemEventId == container.ItemEventId &&
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
                x => x.ContainerEventId == item.ContainerEventId &&
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
                Guid.Empty,
                Guid.Empty,
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
            Guid itemEventId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            var containerToPutawayItemIn = dBContext.Containers.FirstOrDefault(x => x.ContainerEventId == container.ContainerEventId);
            var itemToPutaway = dBContext.Items.FirstOrDefault(x => x.ItemEventId == container.ItemEventId);

            if (containerToPutawayItemIn != null && itemToPutaway != null)
            {
                containerToPutawayItemIn.NextContainerEventId = containerEventId;
                itemToPutaway.NextItemEventId = itemEventId;

                Item newItem = new Item(
                    itemEventId,
                    itemToPutaway.ItemId,
                    itemToPutaway.Name,
                    itemToPutaway.Description,
                    containerEventId,
                    Guid.Empty,
                    dateTimeNow,
                    2,
                    itemToPutaway.ItemEventId,
                    Guid.Empty
                );

                Container newContainer = new Container(
                    containerEventId, 
                    containerToPutawayItemIn.ContainerId, 
                    containerToPutawayItemIn.Name,
                    itemEventId,
                    dateTimeNow, 
                    2, 
                    containerToPutawayItemIn.ContainerEventId, 
                    Guid.Empty
                );
                dBContext.Entry(newItem).State = EntityState.Added;
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
                dBContext.Entry(item).State = EntityState.Added;
            }

            Order order = new Order(Guid.NewGuid(), orderId, 5, dateTimeNow, 0, itemsInOrder.Count, Guid.Empty, Guid.Empty);
            
            dBContext.Orders.Add(order);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PickItem")]
        public async Task<StatusCodeResult> PickItem(ItemContainer itemContainer)
        {
            Guid containerEventId = Guid.NewGuid();
            Guid itemEventId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            var containerToPickItemFrom = dBContext.Containers.FirstOrDefault(x => x.ContainerEventId == itemContainer.Container.ContainerEventId);
            var itemToPick = dBContext.Items.FirstOrDefault(x => x.ItemEventId == itemContainer.Item.ItemEventId);

            if (containerToPickItemFrom != null && itemToPick != null)
            {
                containerToPickItemFrom.NextContainerEventId = containerEventId;
                itemToPick.NextItemEventId = itemEventId;

                Item newItem = new Item(
                    itemEventId,
                    itemToPick.ItemId,
                    itemToPick.Name,
                    itemToPick.Description,
                    Guid.Empty,
                    itemToPick.OrderId,
                    dateTimeNow,
                    4,
                    itemToPick.ItemEventId,
                    Guid.Empty
                );

                Container newContainer = new Container(
                    containerEventId,
                    containerToPickItemFrom.ContainerId,
                    containerToPickItemFrom.Name,
                    Guid.Empty,
                    dateTimeNow,
                    4,
                    containerToPickItemFrom.ContainerEventId,
                    Guid.Empty
                );

                if (newItem.OrderId != Guid.Empty)
                {
                    Guid newOrderEventIdGuid = Guid.NewGuid();
                    var orderToUpdate = dBContext.Orders.FirstOrDefault(x => x.OrderId == newItem.OrderId && x.NextOrderEventId == Guid.Empty);
                    orderToUpdate.NextOrderEventId = newOrderEventIdGuid;

                    Order newOrder = new Order(
                        newOrderEventIdGuid,
                        orderToUpdate.OrderId,
                        6,
                        dateTimeNow,
                        orderToUpdate.NumberOfItemsPickedForOrder + 1,
                        orderToUpdate.TotalNumberOfItemsInOrder,
                        orderToUpdate.OrderEventId,
                        Guid.Empty
                    );
                    dBContext.Entry(newOrder).State = EntityState.Added;
                }

                dBContext.Entry(newItem).State = EntityState.Added;
                dBContext.Entry(newContainer).State = EntityState.Added;
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
