using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private MyDbContext dBContext;

        public OrderController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetOrderByContainerId/{containerId}")]
        public WarehouseObjectWithChildren GetOrderByContainerId(Guid containerId)
        {
            var order = dBContext.WarehouseObjects.FirstOrDefault(x => x.ParentId == containerId && x.NextEventId == Guid.Empty && x.ObjectType == 3);
            var items = dBContext.WarehouseObjects.Where(x => x.OrderId == order.ObjectId && x.NextEventId == Guid.Empty && x.ObjectType == 0).ToList();

            return new WarehouseObjectWithChildren(order, items);
        }

        [HttpPost("RegisterOrder")]
        public async Task<StatusCodeResult> CreateOrder(List<WarehouseObject> itemsInOrder)
        {
            var itemsToUpdateNextEventIdOn = dBContext.WarehouseObjects.Where(x => itemsInOrder.Contains(x));
            await itemsToUpdateNextEventIdOn.ForEachAsync(x => x.NextEventId = Guid.NewGuid());

            Guid orderId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            string orderName = Math.Floor(DateTime.Now.Subtract(new DateTime(2020, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string orderDescription = "Order Containing: ";
            foreach (WarehouseObject item in itemsInOrder)
            {
                item.OrderId = orderId;
                item.EventDateTime = dateTimeNow;
                item.EventType = Constants.ITEM_ADDED_TO_ORDER;
                item.PreviousEventId = item.EventId;
                item.EventId = itemsToUpdateNextEventIdOn.FirstOrDefault(x => x.EventId == item.EventId).NextEventId;
                orderDescription += item.Name + ", ";

                dBContext.Entry(item).State = EntityState.Added;
            }

            WarehouseObject orderDetail = new WarehouseObject(
                Guid.NewGuid(), 
                orderId,
                3,
                orderName,
                orderDescription,
                null,
                null,
                dateTimeNow,
                Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION, 
                Guid.Empty, 
                Guid.Empty
            );

            dBContext.WarehouseObjects.Add(orderDetail);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
