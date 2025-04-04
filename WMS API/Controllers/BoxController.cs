using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.Boxes;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoxController : ControllerBase
    {
        private MyDbContext dBContext;

        public BoxController(MyDbContext context)
        {
            dBContext = context;
        }

        //GET
        [HttpGet("GetAllBoxes")]
        public IList<Box> GetAllBoxes()
        {
            return dBContext.Boxes.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetBoxById/{boxId}")]
        public Box GetBoxById(Guid boxId)
        {
            return dBContext.Boxes.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == boxId);
        }
    }
}
