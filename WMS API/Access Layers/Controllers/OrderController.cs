using Microsoft.AspNetCore.Mvc;
using WMS_API.Access_Layers.Attributes;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Orders;

namespace WMS_API.Layers.Controllers
{
    [ApiKeyAuth]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(
            IOrderService orderService
        )
        {
            _orderService = orderService;
        }

        //GET
        [HttpGet("GetAllOrdersMostRecentData")]
        public async Task<IActionResult> GetAllOrdersMostRecentData()
        {
            try
            {
                var result = await _orderService.GetAllOrdersMostRecentDataAsync();
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetOrderById/{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            try
            {
                var result = await _orderService.GetOrderByIdAsync(orderId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetOrderHistoryById/{orderId}")]
        public async Task<IActionResult> GetOrderHistoryById(Guid orderId)
        {
            try
            {
                var result = await _orderService.GetOrderHistoryByIdAsync(orderId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }

        //POST
        [HttpPost("RegisterOrder")]
        public async Task<IActionResult> RegisterOrder(UnregisteredOrder unregisteredOrder)
        {
            try
            {
                await _orderService.RegisterOrderAsync(unregisteredOrder);
                return Ok();
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("AddBoxToOrder/{orderId}/{boxId}")]
        public async Task<IActionResult> AddBoxToOrder(Guid orderId, Guid boxId)
        {
            try
            {
                var result = await _orderService.AddBoxToOrderAsync(orderId, boxId);
                return Ok(result);
            }
            catch
            {
                return null;
            }
        }
    }
}
