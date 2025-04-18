using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Boxes;
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
            return dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.Acknowledged == false);
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
                false,
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

        [HttpPost("AddContainerToOrder/{orderId}/{containerId}")]
        public async Task<Order> AddContainerToOrder(Guid orderId, Guid containerId)
        {
            var containerDataToUpdate = dBContext.ContainerData.FirstOrDefault(x => x.NextEventId == null && x.ContainerId == containerId);
            var orderDataToUpdate = dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == orderId);

            if (containerDataToUpdate != null && orderDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                Guid newOrderDataEventId = Guid.NewGuid();
                orderDataToUpdate.NextEventId = newOrderDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    orderDataToUpdate.OrderId,
                    newContainerDataEventId,
                    null,
                    containerDataToUpdate.EventId
                );

                OrderData newOrderData = new OrderData(
                    DateTime.Now,
                    orderDataToUpdate.Name,
                    orderDataToUpdate.Description,
                    true,
                    orderDataToUpdate.OrderId,
                    newOrderDataEventId,
                    null,
                    orderDataToUpdate.EventId
                );

                dBContext.OrderData.Add(newOrderData);
                dBContext.ContainerData.Add(newContainerData);

                await dBContext.SaveChangesAsync();

                return dBContext.Orders
                    .Include(x => x.OrderDataHistory)
                    .Include(x => x.OrderItems)
                    .Include(x => x.Address)
                    .Include(x => x.ContainerUsedToPickOrder)
                    .FirstOrDefault(x => x.Id == orderId);
            }
            return null;
        }

        [HttpPost("AddBoxToOrder/{orderId}/{boxId}")]
        public async Task<Order> AddBoxToOrder(Guid orderId, Guid boxId)
        {
            var orderDataToUpdate = dBContext.OrderData.FirstOrDefault(x => x.NextEventId == null && x.OrderId == orderId);
            var boxDataToUpdate = dBContext.BoxData.FirstOrDefault(x => x.NextEventId == null && x.BoxId == boxId);

            if (boxDataToUpdate != null && orderDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newBoxDataEventId = Guid.NewGuid();
                boxDataToUpdate.NextEventId = newBoxDataEventId;

                BoxData newBoxData = new BoxData(
                    dateTimeNow,
                    boxDataToUpdate.Name,
                    boxDataToUpdate.Description,
                    boxDataToUpdate.LengthInCentimeters,
                    boxDataToUpdate.WidthInCentimeters,
                    boxDataToUpdate.HeightInCentimeters,
                    boxDataToUpdate.BoxId,
                    orderDataToUpdate.OrderId,
                    newBoxDataEventId,
                    null,
                    boxDataToUpdate.EventId
                );

                dBContext.BoxData.Add(newBoxData);

                await dBContext.SaveChangesAsync();

                return dBContext.Orders
                    .Include(x => x.OrderDataHistory)
                    .Include(x => x.OrderItems)
                    .Include(x => x.Address)
                    .Include(x => x.ContainerUsedToPickOrder)
                    .FirstOrDefault(x => x.Id == orderId);
            }
            return null;
        }
        
        [HttpPost("RemoveContainerFromOrder/{containerId}")]
        public async Task<StatusCodeResult> RemoveContainerFromOrder(Guid containerId)
        {
            var containerDataToUpdate = dBContext.ContainerData.FirstOrDefault(x => x.NextEventId == null && x.ContainerId == containerId);

            if (containerDataToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newContainerDataEventId = Guid.NewGuid();
                containerDataToUpdate.NextEventId = newContainerDataEventId;

                ContainerData newContainerData = new ContainerData(
                    dateTimeNow,
                    containerDataToUpdate.Name,
                    containerDataToUpdate.Description,
                    containerDataToUpdate.ContainerId,
                    null,
                    newContainerDataEventId,
                    null,
                    containerDataToUpdate.EventId
                );

                dBContext.ContainerData.Add(newContainerData);

                await dBContext.SaveChangesAsync();

                return StatusCode(200);
            }
            return null;
        }
    }
}
