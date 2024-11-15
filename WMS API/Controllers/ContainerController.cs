using Microsoft.AspNetCore.Http.Features;
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
            return dBContext.Containers.ToList();
        }

        [HttpGet("GetContainerById/{itemId}")]
        public Container GetContainerById(Guid containerId)
        {
            return dBContext.Containers.FirstOrDefault(x => x.ContainerId == containerId);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(ContainerToRegister containerToRegister)
        {
            Container container = new Container(
                Guid.NewGuid(),
                containerToRegister.Name,
                new List<Item>(),
                false,
                DateTime.Now,
                Constants.CONTAINER_REGISTERED
            );

            dBContext.Containers.Add(container);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateContainer")]
        public async Task<Container> UpdateContainer(Container container)
        {
            var containerToUpdate = dBContext.Containers.FirstOrDefault(x => x.ContainerId == container.ContainerId);
            var containerHistoryToUpdate = dBContext.ContainerHistory.FirstOrDefault(x => x.ContainerId == container.ContainerId && x.NextContainerEventId == Guid.Empty);

            if (containerToUpdate != null)
            {
                Guid newContainerHistoryEventId = Guid.NewGuid();

                if (containerHistoryToUpdate != null) {
                    containerHistoryToUpdate.NextContainerEventId = newContainerHistoryEventId;
                }
                ContainerHistory containerHistoryEvent = new ContainerHistory(
                    newContainerHistoryEventId,
                    containerToUpdate.ContainerId,
                    containerToUpdate.Name,
                    containerToUpdate.Items,
                    containerToUpdate.IsFull,
                    containerToUpdate.EventDateTime,
                    containerToUpdate.EventType,
                    containerHistoryToUpdate == null ? Guid.Empty : containerHistoryToUpdate.ContainerEventId,
                    Guid.Empty
                );

                containerToUpdate.Name = container.Name;
                containerToUpdate.Items = container.Items;
                containerToUpdate.IsFull = container.IsFull;
                containerToUpdate.EventDateTime = container.EventDateTime;
                containerToUpdate.EventType = container.EventType;

                dBContext.Entry(containerHistoryEvent).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return containerToUpdate;
            }
            return null;
        }
    }
}
