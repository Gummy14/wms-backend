using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Items;
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
        public async Task<StatusCodeResult> CreateOrder(List<Item> itemsInOrder)
        {
            var itemsToUpdateNextEventIdOn = dBContext.Items.Where(x => itemsInOrder.Contains(x));
            await itemsToUpdateNextEventIdOn.ForEachAsync(x => x.NextEventId = Guid.NewGuid());

            Guid orderId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            string orderName = Math.Floor(DateTime.Now.Subtract(new DateTime(2020, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string orderDescription = "Order Containing: ";

            Order newOrder = new Order(
                Guid.NewGuid(),
                orderId,
                orderName,
                orderDescription,
                dateTimeNow,
                Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION,
                Guid.Empty,
                Guid.Empty
            );

            foreach (Item item in itemsInOrder)
            {
                item.EventDateTime = dateTimeNow;
                item.Status = Constants.ITEM_ADDED_TO_ORDER;
                item.PreviousEventId = item.EventId;
                item.EventId = itemsToUpdateNextEventIdOn.FirstOrDefault(x => x.EventId == item.EventId).NextEventId;
                item.OrderId = orderId;
                orderDescription += item.Name + ", ";

                dBContext.Entry(item).State = EntityState.Added;
            }

            dBContext.Orders.Add(newOrder);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
