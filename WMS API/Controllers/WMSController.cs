using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WMS_API.DbContexts;
using WMS_API.Migrations;
using WMS_API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private MyDbContext dBContext;

        public ItemController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetAllItems")]
        public IList<Item> GetAllItems()
        {
            return dBContext.Items.ToList();
        }

        [HttpGet("GetAllRegisteredItems")]
        public IList<Item> GetAllRegisteredItems()
        {
            return dBContext.Items.ToList();
        }

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(Guid itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.Id == itemId);
        }

        [HttpGet("GetContentsOfContainerById/{containerId}")]
        public Item GetContentsOfContainerById(Guid containerId)
        {
            return dBContext.Items.FirstOrDefault(x => x.Id == dBContext.Containers.FirstOrDefault(y => y.Id == containerId).ItemId);
        }

        [HttpGet("GetPutawayLocation")]
        public Container GetPutawayLocation()
        {
            return dBContext.Containers.Where(x => x.ItemId == null).FirstOrDefault();
        }

        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(ItemToRegister itemToRegister)
        {
            Guid guid = Guid.NewGuid();
            Item item = new Item(guid, itemToRegister.Name, itemToRegister.Description);

            dBContext.Items.Add(item);

            AddItemContainerEvent(guid, Guid.Empty, 1);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(ContainerToRegister containerToRegister)
        {
            Guid guid = Guid.NewGuid();
            Container container = new Container(guid, containerToRegister.Name);

            dBContext.Containers.Add(container);

            AddItemContainerEvent(Guid.Empty, guid, 1);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PutawayItem")]
        public async Task<StatusCodeResult> PutawayItem(EventData putawayData)
        {
            var containerToPutawayItemIn = dBContext.Containers.FirstOrDefault(x => x.Id == putawayData.ContainerId);

            if (containerToPutawayItemIn != null) 
            {
                containerToPutawayItemIn.ItemId = putawayData.ItemId;
                AddItemContainerEvent(putawayData.ItemId, putawayData.ContainerId, 2);
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PickItem")]
        public async Task<StatusCodeResult> PickItem(EventData pickData)
        {
            var containerToPickFrom = dBContext.Containers.FirstOrDefault(x => x.Id == pickData.ContainerId);

            if (containerToPickFrom != null)
            {
                containerToPickFrom.ItemId = null;
                AddItemContainerEvent(pickData.ItemId, pickData.ContainerId, 3);
            };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        protected void AddItemContainerEvent(Guid itemId, Guid containerId, int eventType)
        {
            Guid eventGuid = Guid.NewGuid();
            ItemContainerEvent itemContainerEvent = new ItemContainerEvent(eventGuid, itemId, containerId, eventType, DateTime.Now);

            dBContext.ItemContainerEvents.Add(itemContainerEvent);
        }
    }
}
