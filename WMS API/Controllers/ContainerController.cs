using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.WarehouseObjects;
using Container = WMS_API.Models.Containers.Container;
using ContainerData = WMS_API.Models.Containers.ContainerData;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using Microsoft.EntityFrameworkCore;
using WMS_API.Models.Boxes;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;

        public ContainerController(MyDbContext context)
        {
            dBContext = context;
            controllerFunctions = new ControllerFunctions();
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

        //POST
        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(UnregisteredObject objectToRegister)
        {
            Guid containerId = Guid.NewGuid();

            ContainerData newContainerData = new ContainerData(
                DateTime.Now,
                objectToRegister.Name,
                objectToRegister.Description,
                containerId,
                null,
                Guid.NewGuid(),
                null,
                null
            );

            Container newContainer = new Container(
                containerId,
                new List<ContainerData>() { newContainerData },
                null
            );

            dBContext.Containers.Add(newContainer);
            await dBContext.SaveChangesAsync();
            controllerFunctions.printQrCode(objectToRegister.ObjectType + "-" + containerId);
            return StatusCode(200);
        }

        [HttpPost("AddContainerToOrder/{orderId}/{containerId}")]
        public async Task<Order> AddContainerToOrder(Guid orderId, Guid containerId)
        {
            var containerDataToUpdate = dBContext.ContainerData.FirstOrDefault(x => x.NextEventId == null && x.ContainerId == containerId);
            var orderDataToUpdate = dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == orderId);

            if (containerDataToUpdate != null && orderDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    orderDataToUpdate.OrderId,
                    newContainerDataEventId,
                    null,
                    containerDataToUpdate.EventId
                );

                dBContext.ContainerData.Add(newContainerData);

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

        [HttpPost("RemoveContainerFromOrder/{containerId}")]
        public async Task<StatusCodeResult> RemoveContainerFromOrder(Guid containerId)
        {
            var containerDataToUpdate = dBContext.ContainerData.FirstOrDefault(x => x.NextEventId == null && x.ContainerId == containerId);
            var boxDataToUpdate = dBContext.BoxData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == containerDataToUpdate.OrderId);

            if (containerDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                Guid newBoxDataEventId = Guid.NewGuid();
                boxDataToUpdate.NextEventId = newBoxDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    null,
                    newContainerDataEventId,
                    null,
                    containerDataToUpdate.EventId
                );

                BoxData newBoxData = new BoxData(
                    dateTimeNow,
                    boxDataToUpdate.Name,
                    boxDataToUpdate.Description,
                    boxDataToUpdate.LengthInCentimeters,
                    boxDataToUpdate.WidthInCentimeters,
                    boxDataToUpdate.HeightInCentimeters,
                    true,
                    boxDataToUpdate.BoxId,
                    boxDataToUpdate.ShipmentId,
                    boxDataToUpdate.TruckId,
                    boxDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                    boxDataToUpdate.EventId
                );

                dBContext.ContainerData.Add(newContainerData);
                dBContext.BoxData.Add(newBoxData);

                await dBContext.SaveChangesAsync();

                return StatusCode(200);
            }
            return null;
        }
    }
}
