using Microsoft.AspNetCore.Mvc;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Orders;

namespace WMS_API.Layers.Controllers
{
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
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var result = await _orderService.GetAllOrdersAsync();
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
        
        [HttpPost("AddContainerToOrder/{orderId}/{containerId}")]
        public async Task<IActionResult> AddContainerToOrder(Guid orderId, Guid containerId)
        {
            try
            {
                await _orderService.AddContainerToOrderAsync(orderId, containerId);
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
                await _orderService.AddBoxToOrderAsync(orderId, boxId);
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
                await _orderService.RemoveContainerFromOrderAsync(containerId);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
