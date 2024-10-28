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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return dBContext.Items.ToList();
        }

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(Guid itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.Id == itemId);
        }

        [HttpGet("GetAllOrders")]
        public IList<Order> GetAllOrders()
        {
            return dBContext.Orders.Include(x => x.OrderItems).ToList();
        }

        [HttpGet("GetPutawayLocation")]
        public Container GetPutawayLocation()
        {
            return dBContext.Containers.Where(x => x.Item.Id == null).FirstOrDefault();
        }

        [HttpGet("GetItemContainerRelationship/{genericId}")]
        public Container GetItemContainerRelationship(Guid genericId)
        {
            var container = dBContext.Containers.Include(x => x.Item).FirstOrDefault(x => x.Id == genericId);
            if(container != null) {
                return container;
            } else {
                return dBContext.Containers.Include(x => x.Item).FirstOrDefault(x => x.Item.Id == genericId);
            }
        }

        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(ItemToRegister itemToRegister)
        {
            Guid guid = Guid.NewGuid();
            Item item = new Item(guid, itemToRegister.Name, itemToRegister.Description, DateTime.Now);

            dBContext.Items.Add(item);

            //AddToEventHistory(guid, Guid.Empty, 1);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(ContainerToRegister containerToRegister)
        {
            Guid guid = Guid.NewGuid();
            Container container = new Container(guid, containerToRegister.Name, DateTime.Now);

            dBContext.Containers.Add(container);

            //AddToEventHistory(guid, Guid.Empty, 2);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PutawayItem")]
        public async Task<StatusCodeResult> PutawayItem(Container container)
        {
            var containerToPutawayItemIn = dBContext.Containers.FirstOrDefault(x => x.Id == container.Id);

            if (containerToPutawayItemIn != null)
            {
                containerToPutawayItemIn.Item = container.Item;
                //containerToPutawayItemIn.DateTimePutaway = DateTime.Now;
                //AddToEventHistory(container.Id, container.Item.Id, 5);
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("CreateOrder")]
        public async Task<StatusCodeResult> CreateOrder(List<Item> itemsInOrder)
        {
            DateTime dateTimeNow = DateTime.Now;
            Order order = new Order(Guid.NewGuid(), itemsInOrder, DateTime.Now);

            foreach(Item item in itemsInOrder)
            {
                dBContext.Items.Attach(item);
                //dBContext.Items.FirstOrDefault(x => x.Id == item.Id).DateTimeAddedToOrder = dateTimeNow;
            }

            dBContext.Orders.Add(order);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PickItem")]
        public async Task<StatusCodeResult> PickItem(Container container)
        {
            var containerToPickItemFrom = dBContext.Containers.Include(x => x.Item).FirstOrDefault(x => x.Id == container.Id);

            if (containerToPickItemFrom != null)
            {
                containerToPickItemFrom.Item = null;
                //containerToPickItemFrom.DateTimePicked = DateTime.Now;
                //AddToEventHistory(container.Id, container.Item.Id, 6);
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        protected void AddToEventHistory(Guid parentId, Guid childId, int eventType)
        {
            Guid eventGuid = Guid.NewGuid();
            EventHistory eventHistory = new EventHistory(eventGuid, parentId, childId, eventType, DateTime.Now);

            dBContext.EventHistory.Add(eventHistory);
        }
    }
}
