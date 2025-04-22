using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Services;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoxController : ControllerBase
    {
        private readonly IBoxService _boxService;

        public BoxController(
            IBoxService boxService
        )
        {
            _boxService = boxService;
        }

        //GET
        [HttpGet("GetAllBoxes")]
        public async Task<IActionResult> GetAllBoxes()
        {
            try
            {
                var result = await _boxService.GetAllBoxesAsync();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetBoxById/{boxId}")]
        public async Task<IActionResult> GetBoxById(Guid boxId)
        {
            try
            {
                var result = await _boxService.GetBoxByIdAsync(boxId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        //POST
        [HttpPost("RegisterBox")]
        public async Task<IActionResult> RegisterBox(UnregisteredObject objectToRegister)
        {
            try
            {
                await _boxService.RegisterBoxAsync(objectToRegister);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("PackItemIntoBox/{itemId}/{boxId}")]
        public async Task<IActionResult> PackItemIntoBox(Guid itemId, Guid boxId)
        {
            try
            {
                await _boxService.PackItemIntoBoxAsync(itemId, boxId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
