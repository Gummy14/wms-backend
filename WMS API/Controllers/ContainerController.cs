using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using Container = WMS_API.Models.Containers.Container;
using ContainerData = WMS_API.Models.Containers.ContainerData;

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
        public IList<ContainerData> GetAllContainers()
        {
            return dBContext.ContainerData.Where(x => x.NextEventId == null).ToList();
        }

        [HttpGet("GetContainerById/{containerId}")]
        public ContainerData GetContainerById(Guid containerId)
        {
            return dBContext.ContainerData.FirstOrDefault(x => x.NextEventId == null && x.ContainerId == containerId);
        }

        [HttpGet("GetContainerHistory/{containerId}")]
        public List<ContainerData> GetContainerHistory(Guid containerId)
        {
            return dBContext.ContainerData.Where(x => x.ContainerId == containerId).ToList();
        }
    }
}
