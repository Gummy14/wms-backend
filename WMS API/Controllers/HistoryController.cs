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

        [HttpGet("GetObjectHistory/{genericId}")]
        public List<Item> GetObjectHistory(Guid genericId)
        {
            List<Item> objectHistory = new List<Item>();
            var allItemEvents = dBContext.Items.Where(x => x.ItemId == genericId);
            var firstEvent = allItemEvents.FirstOrDefault(x => x.PreviousItemEventId == Guid.Empty);
            objectHistory.Add(firstEvent);

            while (objectHistory.LastOrDefault().NextItemEventId != Guid.Empty)
            {
                var nextEvent = allItemEvents.FirstOrDefault(x => x.ItemEventId == objectHistory.LastOrDefault().NextItemEventId);
                objectHistory.Add(nextEvent);
            }
            return objectHistory;
        }
    }
}
