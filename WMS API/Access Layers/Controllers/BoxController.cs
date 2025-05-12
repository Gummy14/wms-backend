using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("GetAllBoxesMostRecentData")]
        public async Task<IActionResult> GetAllBoxesMostRecentData()
        {
            try
            {
                var result = await _boxService.GetAllBoxesMostRecentDataAsync();
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

        [HttpGet("GetBoxHistoryById/{boxId}")]
        public async Task<IActionResult> GetBoxHistoryById(Guid boxId)
        {
            try
            {
                var result = await _boxService.GetBoxHistoryByIdAsync(boxId);
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
                var result = await _boxService.PackItemIntoBoxAsync(itemId, boxId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }
    }
}
