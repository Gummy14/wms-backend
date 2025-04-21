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

        //[HttpGet("GetNextOrderWaitingForPicking")]
        //public Order GetNextOrderWaitingForPicking()
        //{
        //    return dBContext.Orders
        //        .Include(x => x.OrderDataHistory)
        //        .Include(x => x.OrderItems)
        //        .Include(x => x.Address)
        //        .Include(x => x.ContainerUsedToPickOrder)
        //        .Include(x => x.BoxUsedToPackOrder)
        //        .FirstOrDefault(x => x.ContainerUsedToPickOrder == null);
        //}

        //POST
        [HttpPost("RegisterOrder")]
        public async Task<StatusCodeResult> RegisterOrder(UnregisteredOrder unregisteredOrder)
        {
            var itemDataToUpdateNextEventIdOn = dBContext.ItemData.Where(x => unregisteredOrder.OrderItems.Contains(x));

            Guid orderId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            string orderName = Math.Floor(DateTime.Now.Subtract(new DateTime(2020, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string orderDescription = "Order Containing: ";
            List<ItemData> newOrderItemData = new List<ItemData>();

            int counter = 0;
            foreach (ItemData itemData in itemDataToUpdateNextEventIdOn.AsEnumerable().ToList())
            {
                Guid newItemDataEventId = Guid.NewGuid();
                itemData.NextEventId = newItemDataEventId;

                ItemData newItemData = new ItemData(
                    dateTimeNow,
                    itemData.Name,
                    itemData.Description,
                    itemData.LengthInCentimeters,
                    itemData.WidthInCentimeters,
                    itemData.HeightInCentimeters,
                    itemData.WeightInKilograms,
                    itemData.ItemId,
                    itemData.LocationId,
                    itemData.ContainerId,
                    orderId,
                    itemData.BoxId,
                    newItemDataEventId,
                    null,
                    itemData.EventId
                );

                if (counter == itemDataToUpdateNextEventIdOn.Count() - 1)
                {
                    orderDescription += itemData.Name;
                }
                else
                {
                    orderDescription += itemData.Name + ", ";
                }
                counter++;

                newOrderItemData.Add(newItemData);
            }

            Address newOrderAddress = new Address(
                Guid.NewGuid(),
                orderId,
                unregisteredOrder.Address.FirstName,
                unregisteredOrder.Address.LastName,
                unregisteredOrder.Address.Street,
                unregisteredOrder.Address.City,
                unregisteredOrder.Address.State,
                unregisteredOrder.Address.Zip
            );

            OrderData newOrderData = new OrderData(
                DateTime.Now,
                orderName,
                orderDescription,
                orderId,
                Guid.NewGuid(),
                null,
                null
            );

            Order newOrder = new Order(
                orderId,
                new List<OrderData>() { newOrderData },
                newOrderItemData,
                newOrderAddress,
                null,
                null
            );

            dBContext.Orders.Add(newOrder);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
