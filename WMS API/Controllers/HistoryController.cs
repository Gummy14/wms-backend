using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.Items;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : ControllerBase
    {
        private MyDbContext dBContext;

        public HistoryController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetItemHistory/{itemId}")]
        public List<Item> GetItemHistory(Guid itemId)
        {
            List<Item> itemHistory = new List<Item>();
            var allItemEvents = dBContext.Items.Where(x => x.Id == itemId);
            var firstItemEvent = allItemEvents.FirstOrDefault(x => x.PreviousEventId == Guid.Empty);
            itemHistory.Add(firstItemEvent);

            while (itemHistory.LastOrDefault().NextEventId != Guid.Empty)
            {
                var nextEvent = allItemEvents.FirstOrDefault(x => x.EventId == itemHistory.LastOrDefault().NextEventId);
                itemHistory.Add(nextEvent);
            }
            return itemHistory;
        }
    }
}
