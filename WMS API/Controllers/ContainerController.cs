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
    }
}
