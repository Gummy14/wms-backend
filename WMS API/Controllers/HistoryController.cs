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

        [HttpGet("GetObjectHistory/{itemId}")]
        public List<WarehouseObject> GetItemHistory(Guid objectId, int objectType)
        {
            List<WarehouseObject> objectHistory = new List<WarehouseObject>();
            var allObjectEvents = new List<WarehouseObject>();

            switch (objectType)
            {
                case 0:
                    allObjectEvents.AddRange(dBContext.WarehouseObjects.Where(x =>
                        x.ObjectType == objectType &&
                        x.ItemId == objectId &&
                        x.NextEventId == Guid.Empty
                    ));
                    break;
                case 1:
                    allObjectEvents.AddRange(dBContext.WarehouseObjects.Where(x =>
                        x.ObjectType == objectType &&
                        x.LocationId == objectId &&
                        x.NextEventId == Guid.Empty
                    ));
                    break;
                case 2:
                    allObjectEvents.AddRange(dBContext.WarehouseObjects.Where(x =>
                        x.ObjectType == objectType &&
                        x.ContainerId == objectId &&
                        x.NextEventId == Guid.Empty
                    ));
                    break;
                case 3:
                    allObjectEvents.AddRange(dBContext.WarehouseObjects.Where(x =>
                        x.ObjectType == objectType &&
                        x.OrderId == objectId &&
                        x.NextEventId == Guid.Empty
                    ));
                    break;
                default:
                    return null;
            }

            var firstObjectEvent = allObjectEvents.FirstOrDefault(x => x.PreviousEventId == Guid.Empty);
            objectHistory.Add(firstObjectEvent);

            while (objectHistory.LastOrDefault().NextEventId != Guid.Empty)
            {
                var nextEvent = allObjectEvents.FirstOrDefault(x => x.EventId == objectHistory.LastOrDefault().NextEventId);
                objectHistory.Add(nextEvent);
            }
            return objectHistory;
        }
    }
}
