using Microsoft.AspNetCore.Mvc;
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

        //GET
        [HttpGet("GetAllOrders")]
        public IList<OrderData> GetAllOrders()
        {
            return dBContext.OrderData.Where(x => x.NextEventId == null).ToList();
        }

        [HttpGet("GetOrderById/{orderId}")]
        public OrderData GetOrderById(Guid orderId)
        {
            return dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == orderId);
        }

        [HttpGet("GetNextOrderWaitingForPicking")]
        public OrderData GetNextOrderWaitingForPicking()
        {
            return dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.Status == Constants.ORDER_REGISTERED_WAITING_FOR_ACKNOWLEDGEMENT);
        }

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
                    Constants.ITEM_ADDED_TO_ORDER,
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
                Constants.ORDER_REGISTERED_WAITING_FOR_ACKNOWLEDGEMENT,
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
                newOrderAddress
            );

            dBContext.Orders.Add(newOrder);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        //[HttpPost("UpdateOrderSelectForPicking/{orderId}")]
        //public async Task<OrderData> UpdateOrder(Guid orderId)
        //{
        //    var orderToUpdate = dBContext.Orders.FirstOrDefault(x => x.Id == orderId);

        //    if (orderToUpdate != null)
        //    {
        //        var orderDataToUpdate = dBContext.OrderData.FirstOrDefault(x => x.EventId == orderToUpdate.OrderData.EventId);

        //        Guid newEventId = Guid.NewGuid();
        //        orderDataToUpdate.NextEventId = newEventId;

        //        OrderData newOrderData = new OrderData(
        //            orderDataToUpdate.Name,
        //            orderDataToUpdate.Description,
        //            newEventId,
        //            DateTime.Now,
        //            Constants.ORDER_ACKNOWLEDGED_PICKING_IN_PROGRESS,
        //            orderToUpdate.OrderData.EventId,
        //            null
        //        );

        //        orderToUpdate.OrderData = newOrderData;

        //        await dBContext.SaveChangesAsync();

        //        return newOrderData;
        //    }
        //    return null;
        //}
    }
}
