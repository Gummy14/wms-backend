using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public IList<OrderItems> GetAllOrders()
        {
            List<OrderItems> orderItems = new List<OrderItems>();
            var orders = dBContext.Orders.Where(x => x.NextOrderEventId == Guid.Empty).ToList();

            foreach (var order in orders)
            {
                var items = dBContext.Items.Where(x => x.OrderId == order.OrderId && x.NextItemEventId == Guid.Empty).ToList();
                orderItems.Add(new OrderItems(order, items));
            }
            return orderItems;
        }

        [HttpGet("GetOrderById/{orderId}")]
        public OrderItems GetOrderById(Guid orderId)
        {
            OrderItems orderItems = new OrderItems(
                dBContext.Orders.FirstOrDefault(x => x.OrderId == orderId && x.NextOrderEventId == Guid.Empty),
                dBContext.Items.Where(x => x.OrderId == orderId && x.NextItemEventId == Guid.Empty).ToList()
            );

            return orderItems;
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

        [HttpGet("GetObjectHistory/{genericId}")]
        public List<Item> GetObjectHistory(Guid genericId)
        {
            List<Item> objectHistory = new List<Item>();
            var allItemEvents = dBContext.Items.Where(x => x.ItemId == genericId);
            var firstEvent = allItemEvents.FirstOrDefault(x => x.PreviousItemEventId == Guid.Empty);
            objectHistory.Add(firstEvent);

            while(objectHistory.LastOrDefault().NextItemEventId != Guid.Empty)
            {
                var nextEvent = allItemEvents.FirstOrDefault(x => x.ItemEventId == objectHistory.LastOrDefault().NextItemEventId);
                objectHistory.Add(nextEvent);
            }
            return objectHistory;
        }

        [HttpGet("GetNextUnacknowledgedOrder")]
        public OrderItems GetNextUnacknowledgedOrder()
        {
            OrderItems orderItems = new OrderItems();
            var order = dBContext.Orders.FirstOrDefault(x => x.OrderStatus == 7 && x.NextOrderEventId == Guid.Empty);
            var items = dBContext.Items.Where(x => x.OrderId == order.OrderId && x.NextItemEventId == Guid.Empty).ToList();

            orderItems.Order = order;
            orderItems.Items = items;

            return orderItems;
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
                    3,
                    itemToPutaway.ItemEventId,
                    Guid.Empty
                );

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
                dBContext.Entry(newItem).State = EntityState.Added;
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

            Guid orderId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            foreach (Item item in itemsInOrder) {
                item.OrderId = orderId;
                item.EventDateTime = dateTimeNow;
                item.EventType = 4;
                item.PreviousItemEventId = item.ItemEventId;
                item.ItemEventId = itemsToUpdateNextEventIdOn.FirstOrDefault(x => x.ItemEventId == item.ItemEventId).NextItemEventId;

                dBContext.Entry(item).State = EntityState.Added;
            }

            Order order = new Order(Guid.NewGuid(), orderId, dateTimeNow, 7, Guid.Empty, Guid.Empty);

            dBContext.Orders.Add(order);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("AcknowledgeOrder")]
        public async Task<OrderItems> AcknowledgeOrder(Order order)
        {
            var orderToAcknowledge = dBContext.Orders
                .FirstOrDefault(x => x.OrderId == order.OrderId && x.NextOrderEventId == Guid.Empty);

            if(orderToAcknowledge != null)
            {
                OrderItems newOrderItems = new OrderItems();

                Guid orderEventId = Guid.NewGuid();
                orderToAcknowledge.NextOrderEventId = orderEventId;
                Order newOrder = new Order(
                    orderEventId,
                    orderToAcknowledge.OrderId,
                    DateTime.Now,
                    8,
                    orderToAcknowledge.OrderEventId,
                    Guid.Empty
                );

                dBContext.Entry(newOrder).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                newOrderItems.Order = newOrder;
                newOrderItems.Items = dBContext.Items
                    .Where(x => x.OrderId == orderToAcknowledge.OrderId && x.NextItemEventId == Guid.Empty)
                    .ToList();

                return newOrderItems;
            }
            return null;
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
                    5,
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
                    6,
                    pickBeforeItemEventId,
                    Guid.Empty
                );

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
