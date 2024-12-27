using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.WarehouseObjects;

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
        public WarehouseObject GetPutawayLocation()
        {
            return dBContext.Locations.FirstOrDefault(x => x.NextEventId == Guid.Empty);
        }
    }
}
