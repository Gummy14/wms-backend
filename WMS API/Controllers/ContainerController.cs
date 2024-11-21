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
    public class ContainerController : ControllerBase
    {
        private MyDbContext dBContext;

        public ContainerController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetAllContainers")]
        public IList<Container> GetAllContainers()
        {
            List<Container> containers = new List<Container>();
            var containerDetails = dBContext.ContainerDetails.Where(x => x.NextContainerEventId == Guid.Empty).ToList();

            foreach (var containerDetail in containerDetails)
            {
                var items = dBContext.Items.Where(x => x.OrderId == containerDetail.ContainerId && x.NextItemEventId == Guid.Empty).ToList();
                containers.Add(new Container(containerDetail, items));
            }
            return containers;
        }

        [HttpGet("GetContainerById/{containerId}")]
        public Container GetContainerById(Guid containerId)
        {
            Container container = new Container(
                dBContext.ContainerDetails.FirstOrDefault(x => x.ContainerId == containerId && x.NextContainerEventId == Guid.Empty),
                dBContext.Items.Where(x => x.ContainerId == containerId && x.NextItemEventId == Guid.Empty).ToList()
            );

            return container;
        }

        [HttpGet("GetContainerDetailById/{containerId}")]
        public ContainerDetail GetContainerDetailById(Guid containerId)
        {
            return dBContext.ContainerDetails.FirstOrDefault(x => x.ContainerId == containerId && x.NextContainerEventId == Guid.Empty);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(ContainerToRegister containerToRegister)
        {
            ContainerDetail container = new ContainerDetail(
                Guid.NewGuid(),
                Guid.NewGuid(),
                containerToRegister.Name,
                false,
                containerToRegister.ContainerType,
                DateTime.Now,
                Constants.CONTAINER_REGISTERED,
                Guid.Empty,
                Guid.Empty
            );

            dBContext.ContainerDetails.Add(container);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateContainerDetail")]
        public async Task<ContainerDetail> UpdateContainer(ContainerDetail container)
        {
            var containerToUpdate = dBContext.ContainerDetails
                .FirstOrDefault(x => x.ContainerId == container.ContainerId && x.NextContainerEventId == Guid.Empty);

            if (containerToUpdate != null)
            {
                Guid newContainerEventId = Guid.NewGuid();
                containerToUpdate.NextContainerEventId = newContainerEventId;
                ContainerDetail newContainer = new ContainerDetail(
                    newContainerEventId,
                    containerToUpdate.ContainerId,
                    container.Name,
                    container.IsFull,
                    container.ContainerType,
                    DateTime.Now,
                    container.EventType,
                    containerToUpdate.ContainerEventId,
                    Guid.Empty
                );

                dBContext.Entry(newContainer).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newContainer;
            }
            return null;
        }
    }
}
