using Microsoft.AspNetCore.Mvc;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        //GET
        [HttpGet("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var result = await _itemService.GetAllItemsAsync();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetItemById/{itemId}")]
        public async Task<IActionResult> GetItemById(Guid itemId)
        {
            try
            {
                var result = await _itemService.GetItemByIdAsync(itemId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetItemHistory/{itemId}")]
        public async Task<IActionResult> GetItemHistory(Guid itemId)
        {
            try
            {
                var result = await _itemService.GetItemHistoryAsync(itemId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        //POST
        [HttpPost("RegisterItem")]
        public async Task<IActionResult> RegisterItem(UnregisteredObject objectToRegister)
        {
            try
            {
                await _itemService.RegisterItemAsync(objectToRegister);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("PutawayItem/{itemId}/{locationId}")]
        public async Task<IActionResult> PutawayItem(Guid itemId, Guid locationId)
        {
            try
            {
                await _itemService.PutawayItemAsync(itemId, locationId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("PickItem/{itemId}/{containerId}")]
        public async Task<IActionResult> PickItem(Guid itemId, Guid containerId)
        {
            try
            {
                await _itemService.PickItemAsync(itemId, containerId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("PackItem/{itemId}/{boxId}")]
        public async Task<IActionResult> PackItem(Guid itemId, Guid boxId)
        {
            try
            {
                await _itemService.PackItemAsync(itemId, boxId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
