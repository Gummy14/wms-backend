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

        [HttpGet("GetItemContainerRelationshipByItemId/{itemId}")]
        public Container GetItemContainerRelationshipByItemId(Guid itemId)
        {
            var container = dBContext.Containers.FirstOrDefault(x => x.ItemId == itemId);
            container.Item = dBContext.Items.FirstOrDefault(x => x.Id == itemId);
            return container;
        }

        [HttpGet("GetItemContainerRelationshipByContainerId/{containerId}")]
        public Container GetItemContainerRelationshipByContainerId(Guid containerId)
        {
            var container = dBContext.Containers.FirstOrDefault(x => x.Id == containerId);
            container.Item = dBContext.Items.FirstOrDefault(x => x.Id == container.ItemId);
            return container;
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

            AddToEventHistory(guid, Guid.Empty, 1);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(ContainerToRegister containerToRegister)
        {
            Guid guid = Guid.NewGuid();
            Container container = new Container(guid, containerToRegister.Name);

            dBContext.Containers.Add(container);

            AddToEventHistory(guid, Guid.Empty, 2);

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
                AddToEventHistory(putawayData.ContainerId, putawayData.ItemId, 5);
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
                AddToEventHistory(pickData.ContainerId, pickData.ItemId, 6);
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
