using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Models.WarehouseObjects;
using Container = WMS_API.Models.Containers.Container;
using ContainerData = WMS_API.Models.Containers.ContainerData;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using Microsoft.EntityFrameworkCore;

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
    }
}
