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

        [HttpPost("AddItemData")]
        public async Task<StatusCodeResult> AddItemData(Item[] itemData)
        {
            foreach(Item item in itemData)
            {
                item.Status = "Received";
                item.PutawayLocation = "A1";
                dBContext.Items.Add(item);
            }
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
