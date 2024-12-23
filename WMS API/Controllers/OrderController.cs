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

        [HttpPost("RegisterOrder")]
        public async Task<StatusCodeResult> CreateOrder(List<WarehouseObject> warehouseObjectsInOrder)
        {
            var itemsToUpdateNextEventIdOn = dBContext.WarehouseObjects.Where(x => warehouseObjectsInOrder.Contains(x));
            await itemsToUpdateNextEventIdOn.ForEachAsync(x => x.NextEventId = Guid.NewGuid());

            Guid orderId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            string orderName = Math.Floor(DateTime.Now.Subtract(new DateTime(2020, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string orderDescription = "Order Containing: ";

            WarehouseObject orderDetail = new WarehouseObject(
                Guid.NewGuid(),
                orderId,
                3,
                orderName,
                orderDescription,
                dateTimeNow,
                Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION,
                Guid.Empty,
                Guid.Empty
            );
            dBContext.WarehouseObjects.Add(orderDetail);

            foreach (WarehouseObject warehouseObject in warehouseObjectsInOrder)
            {
                warehouseObject.EventDateTime = dateTimeNow;
                warehouseObject.Status = Constants.ITEM_ADDED_TO_ORDER;
                warehouseObject.PreviousEventId = warehouseObject.EventId;
                warehouseObject.EventId = itemsToUpdateNextEventIdOn.FirstOrDefault(x => x.EventId == warehouseObject.EventId).NextEventId;
                orderDescription += warehouseObject.Name + ", ";

                dBContext.Entry(warehouseObject).State = EntityState.Added;
                dBContext.WarehouseObjectRelationships.Add(new WarehouseObjectRelationship(Guid.NewGuid(), Guid.NewGuid(), orderId, warehouseObject.ObjectId, dateTimeNow, Guid.Empty, Guid.Empty));
            }

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
