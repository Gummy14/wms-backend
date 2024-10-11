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

        [HttpPost("ReceiveItem")]
        public async Task<StatusCodeResult> ReceiveItem(ReceivedItem receivedItem)
        {
            Item item = new Item(receivedItem.Name,receivedItem.Description,"Received", "A1");

            dBContext.Items.Add(item);
           
            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PutawayItem")]
        public async Task<StatusCodeResult> PutawayItem(Item itemToPutaway)
        {
            var data = dBContext.Items.FirstOrDefault(x => x.Id == itemToPutaway.Id);
            
            if(data != null) { data.Status = "Putaway"; };

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
