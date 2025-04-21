using Microsoft.AspNetCore.Mvc;
using WMS_API.Models.WarehouseObjects;
using WMS_API.Layers.Services.Interfaces;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _containerService;

        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        //GET
        [HttpGet("GetAllContainers")]
        public async Task<IActionResult> GetAllContainers()
        {
            try
            {
                var result = await _containerService.GetAllContainersAsync();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetContainerById/{containerId}")]
        public async Task<IActionResult> GetContainerById(Guid containerId)
        {
            try
            {
                var result = await _containerService.GetContainerByIdAsync(containerId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetContainerHistory/{containerId}")]
        public async Task<IActionResult> GetContainerHistory(Guid containerId)
        {
            try
            {
                var result = await _containerService.GetContainerHistoryAsync(containerId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        //POST
        [HttpPost("RegisterContainer")]
        public async Task<IActionResult> RegisterContainer(UnregisteredObject objectToRegister)
        {
            try
            {
                await _containerService.RegisterContainerAsync(objectToRegister);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("AddContainerToOrder/{orderId}/{containerId}")]
        public async Task<IActionResult> AddContainerToOrder(Guid orderId, Guid containerId)
        {
            try
            {
                await _containerService.AddContainerToOrderAsync(orderId, containerId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("RemoveContainerFromOrder/{containerId}")]
        public async Task<IActionResult> RemoveContainerFromOrder(Guid containerId)
        {
            try
            {
                await _containerService.RemoveContainerFromOrderAsync(containerId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
