using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using Container = WMS_API.Models.Containers.Container;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private MyDbContext dBContext;

        public ContainerController(MyDbContext context)
        {
            dBContext = context;
        }

        //GET
        [HttpGet("GetAllContainers")]
        public IList<Container> GetAllContainers()
        {
            return dBContext.Containers.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetContainerById/{containerId}")]
        public Container GetContainerById(Guid containerId)
        {
            return dBContext.Containers.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == containerId);
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
