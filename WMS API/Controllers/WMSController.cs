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

        [HttpPost("AddItem")]
        public async Task<StatusCodeResult> AddNewItem(Item item)
        {
            dBContext.Items.Add(item);
            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
