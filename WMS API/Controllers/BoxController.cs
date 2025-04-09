using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IList<BoxData> GetAllBoxes()
        {
            return dBContext.BoxData.Where(x => x.NextEventId == null).ToList();
        }

        [HttpGet("GetBoxById/{boxId}")]
        public BoxData GetBoxById(Guid boxId)
        {
            return dBContext.BoxData.FirstOrDefault(x => x.NextEventId == null && x.BoxId == boxId);
        }
    }
}
