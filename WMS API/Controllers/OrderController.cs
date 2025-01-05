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

        //GET
        [HttpGet("GetAllOrders")]
        public IList<Order> GetAllOrders()
        {
            List<Order> allOrders = dBContext.Orders.Where(x => x.NextEventId == Guid.Empty).ToList();

            foreach (Order order in allOrders)
            {
                order.OrderItems = dBContext.Items.Where(x => x.NextEventId == Guid.Empty && x.OrderId == order.Id).ToList();
            }

            return allOrders;
        }

        [HttpGet("GetOrderById/{orderId}")]
        public Order GetOrderById(Guid orderId)
        {
            var order = dBContext.Orders.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == orderId);
            order.OrderItems = dBContext.Items.Where(x => x.NextEventId == Guid.Empty && x.OrderId == order.Id).ToList();

            return order;
        }

        [HttpGet("GetNextOrderWaitingForPicking")]
        public Order GetNextOrderWaitingForPicking()
        {
            var order = dBContext.Orders.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Status == Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION);
            order.OrderItems = dBContext.Items.Where(x => x.NextEventId == Guid.Empty && x.OrderId == order.Id).ToList();

            return order;
        }

        //POST
        [HttpPost("RegisterOrder")]
        public async Task<StatusCodeResult> CreateOrder(List<Item> itemsInOrder)
        {
            var itemsToUpdateNextEventIdOn = dBContext.Items.Where(x => itemsInOrder.Contains(x));
            await itemsToUpdateNextEventIdOn.ForEachAsync(x => x.NextEventId = Guid.NewGuid());

            Guid orderId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            string orderName = Math.Floor(DateTime.Now.Subtract(new DateTime(2020, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string orderDescription = "Order Containing: ";

            int counter = 0;
            foreach (Item item in itemsInOrder)
            {
                item.DateTimeStamp = dateTimeNow;
                item.Status = Constants.ITEM_ADDED_TO_ORDER;
                item.PreviousEventId = item.EventId;
                item.EventId = itemsToUpdateNextEventIdOn.FirstOrDefault(x => x.EventId == item.EventId).NextEventId;
                item.OrderId = orderId;
                if (counter == itemsInOrder.Count - 1)
                {
                    orderDescription += item.Name;
                }
                else
                {
                    orderDescription += item.Name + ", ";
                }
                counter++;

                dBContext.Entry(item).State = EntityState.Added;
            }

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

            dBContext.Orders.Add(newOrder);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateOrderSelectForPicking/{orderId}")]
        public async Task<Order> UpdateOrder(Guid orderId)
        {
            var orderToUpdate = dBContext.Orders.FirstOrDefault(x => x.Id == orderId && x.NextEventId == Guid.Empty);

            if (orderToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                orderToUpdate.NextEventId = newEventId;
                Order newOrder = new Order(
                    newEventId,
                    orderToUpdate.Id,
                    orderToUpdate.Name,
                    orderToUpdate.Description,
                    DateTime.Now,
                    Constants.ORDER_SELECTED_FOR_PICKING_PICKING_IN_PROGRESS,
                    orderToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newOrder).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                newOrder.OrderItems = dBContext.Items.Where(x => x.NextEventId == Guid.Empty && x.OrderId == orderId).ToList();

                return newOrder;
            }
            return null;
        }
    }
}
