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
            return dBContext.Containers.Where(x => x.NextContainerEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetContainerById/{itemId}")]
        public Container GetContainerById(Guid containerId)
        {
            return dBContext.Containers.FirstOrDefault(x => x.ContainerId == containerId && x.NextContainerEventId == Guid.Empty);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(ContainerToRegister containerToRegister)
        {
            Container container = new Container(
                Guid.NewGuid(),
                Guid.NewGuid(),
                containerToRegister.Name,
                Guid.Empty,
                DateTime.Now,
                Constants.CONTAINER_REGISTERED,
                Guid.Empty,
                Guid.Empty
            );

            dBContext.Containers.Add(container);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateContainer")]
        public async Task<Container> UpdateContainer(Container container)
        {
            var containerToUpdate = dBContext.Containers
                .FirstOrDefault(x => x.ContainerId == container.ContainerId && x.NextContainerEventId == Guid.Empty);

            if (containerToUpdate != null)
            {
                Guid newContainerEventId = Guid.NewGuid();
                containerToUpdate.NextContainerEventId = newContainerEventId;
                Container newContainer = new Container(
                    newContainerEventId,
                    containerToUpdate.ContainerId,
                    container.Name,
                    container.ItemId,
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
