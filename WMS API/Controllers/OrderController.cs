using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;

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

        [HttpGet("GetAllOrders")]
        public IList<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            var orderDetails = dBContext.OrderDetails.Where(x => x.NextOrderEventId == Guid.Empty).ToList();

            foreach (var orderDetail in orderDetails)
            {
                var items = dBContext.Items.Where(x => x.OrderId == orderDetail.OrderId && x.NextItemEventId == Guid.Empty).ToList();
                orders.Add(new Order(orderDetail, items));
            }
            return orders;
        }

        [HttpGet("GetOrderById/{orderId}")]
        public Order GetOrderById(Guid orderId)
        {
            Order orderItems = new Order(
                dBContext.OrderDetails.FirstOrDefault(x => x.OrderId == orderId && x.NextOrderEventId == Guid.Empty),
                dBContext.Items.Where(x => x.OrderId == orderId && x.NextItemEventId == Guid.Empty).ToList()
            );

            return orderItems;
        }

        [HttpGet("GetNextOrderByStatus/{orderStatus}")]
        public Order GetNextOrderByStatus(int orderStatus)
        {
            Order order = new Order();
            var orderDetail = dBContext.OrderDetails.FirstOrDefault(x => x.OrderStatus == orderStatus && x.NextOrderEventId == Guid.Empty);
            var items = dBContext.Items.Where(x => x.OrderId == orderDetail.OrderId && x.NextItemEventId == Guid.Empty).ToList();

            order.OrderDetail = orderDetail;
            order.Items = items;

            return order;
        }

        [HttpPost("RegisterOrder")]
        public async Task<StatusCodeResult> CreateOrder(List<Item> itemsInOrder)
        {
            var itemsToUpdateNextEventIdOn = dBContext.Items.Where(x => itemsInOrder.Contains(x));
            await itemsToUpdateNextEventIdOn.ForEachAsync(x => x.NextItemEventId = Guid.NewGuid());

            Guid orderId = Guid.NewGuid();
            DateTime dateTimeNow = DateTime.Now;
            foreach (Item item in itemsInOrder)
            {
                item.OrderId = orderId;
                item.EventDateTime = dateTimeNow;
                item.EventType = Constants.ITEM_ADDED_TO_ORDER;
                item.PreviousItemEventId = item.ItemEventId;
                item.ItemEventId = itemsToUpdateNextEventIdOn.FirstOrDefault(x => x.ItemEventId == item.ItemEventId).NextItemEventId;

                dBContext.Entry(item).State = EntityState.Added;
            }

            OrderDetail orderDetail = new OrderDetail(Guid.NewGuid(), orderId, dateTimeNow, Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION, Guid.Empty, Guid.Empty);

            dBContext.OrderDetails.Add(orderDetail);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateOrderDetail")]
        public async Task<OrderDetail> UpdateOrderDetail(OrderDetail newOrderDetail)
        {
            var orderToAcknowledge = dBContext.OrderDetails
                .FirstOrDefault(x => x.OrderId == newOrderDetail.OrderId && x.NextOrderEventId == Guid.Empty);

            if (orderToAcknowledge != null)
            {
                Guid orderEventId = Guid.NewGuid();
                orderToAcknowledge.NextOrderEventId = orderEventId;
                OrderDetail orderDetailToAdd = new OrderDetail(
                    orderEventId,
                    orderToAcknowledge.OrderId,
                    DateTime.Now,
                    newOrderDetail.OrderStatus,
                    orderToAcknowledge.OrderEventId,
                    Guid.Empty
                );

                dBContext.Entry(orderDetailToAdd).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return orderDetailToAdd;
            }
            return null;
        }
    }
}
