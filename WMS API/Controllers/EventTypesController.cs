using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.Events;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventTypesController : ControllerBase
    {
        private MyDbContext dBContext;

        public EventTypesController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetAllEventTypes")]
        public IList<EventType> GetAllEventTypes()
        {
            return dBContext.EventTypes.ToList();
        }
    }
}
