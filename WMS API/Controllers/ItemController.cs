using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Items;
using ZXing;
using ZXing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;
using System.Text.Json;

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

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 200,
                    Width = 200,
                    Margin = 1
                }
            };

            var result = writer.Write(JsonSerializer.Serialize(itemToRegister));
            var barcodeBitmap = new Bitmap(result);

            barcodeBitmap.Save(@"C:\Users\alexh\OneDrive\Pictures\" + itemId.ToString() + ".png", ImageFormat.Png);

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
