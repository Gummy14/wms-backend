using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Containers;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;

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

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(Guid itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.ItemId == itemId);
        }

        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(ItemToRegister itemToRegister)
        {
            Item item = new Item(
                Guid.NewGuid(),
                itemToRegister.Name,
                itemToRegister.Description,
                Guid.Empty,
                null,
                DateTime.Now,
                Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION
            );

            dBContext.Items.Add(item);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateItem")]
        public async Task<Item> UpdateItem(Item item)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.ItemId == item.ItemId);
            var itemHistoryToUpdate = dBContext.ItemHistory.FirstOrDefault(x => x.ItemId == item.ItemId && x.NextItemEventId == Guid.Empty);

            if (itemToUpdate != null)
            {
                Guid newItemHistoryEventId = Guid.NewGuid();

                if (itemHistoryToUpdate != null)
                {
                    itemHistoryToUpdate.NextItemEventId = newItemHistoryEventId;
                }
                ItemHistory itemHistoryEvent = new ItemHistory(
                    newItemHistoryEventId,
                    itemToUpdate.ItemId,
                    itemToUpdate.Name,
                    itemToUpdate.Description,
                    itemToUpdate.ContainerId,
                    itemToUpdate.OrderId,
                    itemToUpdate.EventDateTime,
                    itemToUpdate.EventType,
                    itemHistoryToUpdate == null ? Guid.Empty : itemHistoryToUpdate.ItemEventId,
                    Guid.Empty
                );

                itemToUpdate.Name = item.Name;
                itemToUpdate.Description = item.Description;
                itemToUpdate.ContainerId = item.ContainerId;
                itemToUpdate.OrderId = item.OrderId;
                itemToUpdate.EventDateTime = DateTime.Now;
                itemToUpdate.EventType = item.EventType;

                dBContext.Entry(itemHistoryEvent).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return itemToUpdate;
            }
            return null;
        }
    }
}
