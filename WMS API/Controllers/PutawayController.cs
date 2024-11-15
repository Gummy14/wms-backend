using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Containers;
using WMS_API.Models.Items;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PutawayController : ControllerBase
    {
        private MyDbContext dBContext;

        public PutawayController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetPutawayLocation")]
        public ContainerDetail GetPutawayLocation()
        {
            return dBContext.ContainerDetails.FirstOrDefault(x => !x.IsFull && x.NextContainerEventId == Guid.Empty);
        }
    }
}
