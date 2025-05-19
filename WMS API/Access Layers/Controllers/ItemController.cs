using Microsoft.AspNetCore.Mvc;
using WMS_API.Access_Layers.Attributes;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Controllers
{
    [ApiKeyAuth]
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(
            IItemService itemService
        )
        {
            _itemService = itemService;
        }

        //GET
        [HttpGet("GetAllItemsMostRecentData")]
        public async Task<IActionResult> GetAllItemsMostRecentData()
        {
            try
            {
                var result = await _itemService.GetAllItemsMostRecentDataAsync();
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

        [HttpGet("GetItemHistoryById/{itemId}")]
        public async Task<IActionResult> GetItemHistoryById(Guid itemId)
        {
            try
            {
                var result = await _itemService.GetItemHistoryByIdAsync(itemId);
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
    }
}
