using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Containers;
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
        public IList<Order> GetAllOrders()
        {
            return dBContext.Orders
                .Include(x => x.OrderDataHistory)
                .Include(x => x.OrderItems)
                .Include(x => x.Address)
                .Include(x => x.ContainerUsedToPickOrder)
                .ToList();
        }

        [HttpGet("GetOrderById/{orderId}")]
        public OrderData GetOrderById(Guid orderId)
        {
            return dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == orderId);
        }

        [HttpGet("GetNextOrderWaitingForPicking")]
        public OrderData GetNextOrderWaitingForPicking()
        {
            return dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.EventType == Constants.ORDER_REGISTERED);
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
                Constants.ORDER_REGISTERED,
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

        [HttpPost("CompletePicking/{orderId}")]
        public async Task<StatusCodeResult> CompletePicking(Guid orderId)
        {
            var orderDataToUpdate = dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == orderId);

            if (orderDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newOrderDataEventId = Guid.NewGuid();
                orderDataToUpdate.NextEventId = newOrderDataEventId;

                OrderData newOrderData = new OrderData(
                    DateTime.Now,
                    Constants.ORDER_PICKING_COMPLETE,
                    orderDataToUpdate.Name,
                    orderDataToUpdate.Description,
                    orderDataToUpdate.OrderId,
                    newOrderDataEventId,
                    null,
                    orderDataToUpdate.EventId
                );

                dBContext.OrderData.Add(newOrderData);

                await dBContext.SaveChangesAsync();

                return StatusCode(200);
            }
            return null;
        }
    }

}
