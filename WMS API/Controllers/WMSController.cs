using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WMS_API.DbContexts;
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
        public Item GetItemById(int itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.Id == itemId);
        }

        [HttpGet("GetPutawayLocation")]
        public Container GetPutawayLocation()
        {
            return dBContext.Containers.Where(x => x.ItemId == null).FirstOrDefault();
        }

        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(ItemToRegister itemToRegister)
        {
            Item item = new Item(itemToRegister.Name, itemToRegister.Description);

            dBContext.Items.Add(item);
           
            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(int containerToRegister)
        {
            Container container = new Container(containerToRegister);

            dBContext.Containers.Add(container);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PutawayItem")]
        public async Task<StatusCodeResult> PutawayItem(PutawayAction putawayAction)
        {
            var containerToPutawayItemIn = dBContext.Containers.FirstOrDefault(x => x.Id == putawayAction.container.Id);

            if (containerToPutawayItemIn != null) { containerToPutawayItemIn.ItemId = putawayAction.item.Id; };

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PickItem")]
        public async Task<StatusCodeResult> PickItem(Item item)
        {
            var data = dBContext.Items.FirstOrDefault(x => x.Id == item.Id);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
