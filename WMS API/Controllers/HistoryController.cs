using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using Container = WMS_API.Models.Containers.Container;

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

        [HttpGet("GetLocationHistory/{locationId}")]
        public List<Location> GetLocationHistory(Guid locationId)
        {
            List<Location> locationHistory = new List<Location>();
            var allLocationEvents = dBContext.Locations.Where(x => x.Id == locationId);
            var firstLocationEvent = allLocationEvents.FirstOrDefault(x => x.PreviousEventId == Guid.Empty);
            locationHistory.Add(firstLocationEvent);

            while (locationHistory.LastOrDefault().NextEventId != Guid.Empty)
            {
                var nextEvent = allLocationEvents.FirstOrDefault(x => x.EventId == locationHistory.LastOrDefault().NextEventId);
                locationHistory.Add(nextEvent);
            }
            return locationHistory;
        }

        [HttpGet("GetContainerHistory/{containerId}")]
        public List<Container> GetContainerHistory(Guid containerId)
        {
            List<Container> containerHistory = new List<Container>();
            var allContainerEvents = dBContext.Containers.Where(x => x.Id == containerId);
            var firstContainerEvent = allContainerEvents.FirstOrDefault(x => x.PreviousEventId == Guid.Empty);
            containerHistory.Add(firstContainerEvent);

            while (containerHistory.LastOrDefault().NextEventId != Guid.Empty)
            {
                var nextEvent = allContainerEvents.FirstOrDefault(x => x.EventId == containerHistory.LastOrDefault().NextEventId);
                containerHistory.Add(nextEvent);
            }
            return containerHistory;
        }
    }
}
