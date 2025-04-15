using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WMS_API.DbContexts;
using WMS_API.Models.Boxes;
using WMS_API.Models.Containers;
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

        [HttpPost("AddBoxToOrder/{orderId}/{boxId}")]
        public async Task<Order> AddContainerToOrder(Guid orderId, Guid boxId)
        {
            var orderDataToUpdate = dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == orderId);
            var boxDataToUpdate = dBContext.BoxData.FirstOrDefault(x => x.NextEventId == null && x.BoxId == boxId);

            if (boxDataToUpdate != null && orderDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newBoxDataEventId = Guid.NewGuid();
                boxDataToUpdate.NextEventId = newBoxDataEventId;

                Guid newOrderDataEventId = Guid.NewGuid();
                orderDataToUpdate.NextEventId = newOrderDataEventId;

                BoxData newBoxData = new BoxData(
                    dateTimeNow,
                    Constants.BOX_ADDED_TO_ORDER,
                    boxDataToUpdate.Name,
                    boxDataToUpdate.Description,
                    boxDataToUpdate.LengthInCentimeters,
                    boxDataToUpdate.WidthInCentimeters,
                    boxDataToUpdate.HeightInCentimeters,
                    boxDataToUpdate.BoxId,
                    orderDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                    boxDataToUpdate.EventId
                );

                OrderData newOrderData = new OrderData(
                    dateTimeNow,
                    Constants.BOX_ADDED_TO_ORDER,
                    orderDataToUpdate.Name,
                    orderDataToUpdate.Description,
                    orderDataToUpdate.OrderId,
                    newOrderDataEventId,
                    null,
                    orderDataToUpdate.EventId
                );

                dBContext.OrderData.Add(newOrderData);
                dBContext.BoxData.Add(newBoxData);

                await dBContext.SaveChangesAsync();

                return dBContext.Orders
                    .Include(x => x.OrderDataHistory)
                    .Include(x => x.OrderItems)
                    .Include(x => x.Address)
                    .Include(x => x.ContainerUsedToPickOrder)
                    .FirstOrDefault(x => x.Id == orderId);
            }
            return null;
        }
    }
}
