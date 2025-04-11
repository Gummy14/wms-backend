using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.WarehouseObjects;
using Container = WMS_API.Models.Containers.Container;
using ContainerData = WMS_API.Models.Containers.ContainerData;
using Constants = WMS_API.Models.Constants;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;

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
                Constants.CONTAINER_NOT_IN_USE,
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
        public async Task<StatusCodeResult> AddContainerToOrder(Guid orderId, Guid containerId)
        {
            var containerDataToUpdate = dBContext.ContainerData.FirstOrDefault(x => x.NextEventId == null && x.ContainerId == containerId);
            var orderDataToUpdate = dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == orderId);

            if (containerDataToUpdate != null && orderDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                Guid newOrderDataEventId = Guid.NewGuid();
                orderDataToUpdate.NextEventId = newOrderDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    Constants.CONTAINER_ADDED_TO_ORDER,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    orderDataToUpdate.OrderId,
                    newContainerDataEventId,
                    null,
                    containerDataToUpdate.EventId
                );

                OrderData newOrderData = new OrderData(
                    DateTime.Now,
                    Constants.ORDER_ACKNOWLEDGED_PICKING_IN_PROGRESS,
                    orderDataToUpdate.Name,
                    orderDataToUpdate.Description,
                    orderDataToUpdate.OrderId,
                    newOrderDataEventId,
                    null,
                    orderDataToUpdate.EventId
                );

                dBContext.OrderData.Add(newOrderData);
                dBContext.ContainerData.Add(newContainerData);

                await dBContext.SaveChangesAsync();

                return StatusCode(200);
            }
            return null;
        }
    }
}
