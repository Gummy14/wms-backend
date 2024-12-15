using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.WarehouseObjects;

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

        [HttpGet("GetObjectHistory/{objectId}")]
        public List<WarehouseObject> GetObjectHistory(Guid objectId)
        {
            List<WarehouseObject> objectHistory = new List<WarehouseObject>();
            var allEvents = dBContext.WarehouseObjects.Where(x => x.ObjectId == objectId);
            var firstEvent = allEvents.FirstOrDefault(x => x.PreviousEventId == Guid.Empty);
            objectHistory.Add(firstEvent);

            while (objectHistory.LastOrDefault().NextEventId != Guid.Empty)
            {
                var nextEvent = allEvents.FirstOrDefault(x => x.EventId == objectHistory.LastOrDefault().NextEventId);
                objectHistory.Add(nextEvent);
            }
            return objectHistory;
        }
    }
}
