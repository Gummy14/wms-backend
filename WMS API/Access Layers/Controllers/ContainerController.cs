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

        public ContainerController(
            IContainerService containerService
        )
        {
            _containerService = containerService;
        }

        //GET
        [HttpGet("GetAllContainersMostRecentData")]
        public async Task<IActionResult> GetAllContainersMostRecentData()
        {
            try
            {
                var result = await _containerService.GetAllContainersMostRecentDataAsync();
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

        [HttpPost("PickItemIntoContainer/{itemId}/{containerId}")]
        public async Task<IActionResult> PickItemIntoContainer(Guid itemId, Guid containerId)
        {
            try
            {
                await _containerService.PickItemIntoContainerAsync(itemId, containerId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
