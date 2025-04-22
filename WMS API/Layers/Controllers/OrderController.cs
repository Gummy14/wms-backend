using Microsoft.AspNetCore.Mvc;
using WMS_API.DbContexts;
using WMS_API.Layers.Controllers.Functions;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;

namespace WMS_API.Layers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;
        private readonly IOrderService _orderService;

        public OrderController(MyDbContext context, IOrderService orderService)
        {
            dBContext = context;
            _orderService = orderService;
            controllerFunctions = new ControllerFunctions();
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

        [HttpGet("GetOrderHistory/{orderId}")]
        public async Task<IActionResult> GetOrderHistory(Guid orderId)
        {
            try
            {
                var result = await _orderService.GetOrderHistoryAsync(orderId);
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
    }
}
