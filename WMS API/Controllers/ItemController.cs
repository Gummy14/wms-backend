using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Items;


namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;

        public ItemController(MyDbContext context)
        {
            dBContext = context;
            controllerFunctions = new ControllerFunctions();
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

        [HttpPost("PrintItemQRCode")]
        public async Task<StatusCodeResult> PrintItemQRCode(ItemToRegister itemToRegister)
        {
            Guid itemId = Guid.NewGuid();
            itemToRegister.ItemId = itemId;
            string registrationString = JsonSerializer.Serialize(itemToRegister);

            controllerFunctions.printQrCodeFromRegistrationString(registrationString);

            return StatusCode(200);
        }

        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(ItemToRegister itemToRegister)
        {
            Item item = new Item(
                Guid.NewGuid(),
                (Guid)itemToRegister.ItemId,
                itemToRegister.Name,
                itemToRegister.Description,
                null,
                null,
                DateTime.Now,
                Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION,
                Guid.Empty,
                Guid.Empty
            );

            dBContext.Items.Add(item);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateItem")]
        public async Task<Item> UpdateItem(Item item)
        {
            var itemToUpdate = dBContext.Items
                .FirstOrDefault(x => x.ItemId == item.ItemId && x.NextItemEventId == Guid.Empty);

            if (itemToUpdate != null)
            {
                Guid newItemEventId = Guid.NewGuid();
                itemToUpdate.NextItemEventId = newItemEventId;
                Item newItem = new(
                    newItemEventId,
                    itemToUpdate.ItemId,
                    item.Name,
                    item.Description,
                    item.ContainerId,
                    item.OrderId,
                    DateTime.Now,
                    item.EventType,
                    itemToUpdate.ItemEventId,
                    Guid.Empty

                );

                dBContext.Entry(newItem).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }
    }
}
