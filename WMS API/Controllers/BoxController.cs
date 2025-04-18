using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WMS_API.DbContexts;
using WMS_API.Models.Boxes;
using WMS_API.Models.Containers;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;
using Constants = WMS_API.Models.Constants;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoxController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;

        public BoxController(MyDbContext context)
        {
            dBContext = context;
            controllerFunctions = new ControllerFunctions();
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

        //POST
        [HttpPost("RegisterBox")]
        public async Task<StatusCodeResult> RegisterBox(UnregisteredObject objectToRegister)
        {
            Guid boxId = Guid.NewGuid();

            BoxData newBoxData = new BoxData(
                DateTime.Now,
                Constants.BOX_REGISTERED,
                objectToRegister.Name,
                objectToRegister.Description,
                objectToRegister.LengthInCentimeters,
                objectToRegister.WidthInCentimeters,
                objectToRegister.HeightInCentimeters,
                boxId,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Box newBox = new Box(
                boxId,
                new List<BoxData>() { newBoxData },
                null
            );

            dBContext.Boxes.Add(newBox);
            await dBContext.SaveChangesAsync();
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + boxId);
            return StatusCode(200);
        }

        [HttpPost("PrintShippingLabel/{boxId}")]
        public async Task<StatusCodeResult> PrintShippingLabel(Guid boxId)
        {
            var boxData = dBContext.BoxData.FirstOrDefault(x => x.NextEventId == null && x.BoxId == boxId);
            var addressToPrint = dBContext.Addresses.FirstOrDefault(x => x.OrderId == boxData.OrderId);
            controllerFunctions.printShippingLabel(addressToPrint);
            return StatusCode(200);
        }
    }
}
