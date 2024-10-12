using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;

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
            return dBContext.Items.Where(x => x.StatusId == 1).ToList();
        }

        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(ItemToRegister itemToRegister)
        {
            Item item = new Item(itemToRegister.Name, itemToRegister.Description, 1, "");

            dBContext.Items.Add(item);
           
            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PutawayItem")]
        public async Task<StatusCodeResult> PutawayItem(Item itemToPutaway)
        {
            var data = dBContext.Items.FirstOrDefault(x => x.Id == itemToPutaway.Id);
            
            if(data != null) { data.StatusId = 2; };

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
