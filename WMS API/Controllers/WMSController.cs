using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;
using Container = WMS_API.Models.Containers.Container;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WMSController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;

        public WMSController(MyDbContext context)
        {
            dBContext = context;
            controllerFunctions = new ControllerFunctions();
        }

        [HttpGet("GetAllItems")]
        public IList<Item> GetAllItems()
        {
            return dBContext.Items.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetAllLocations")]
        public IList<Location> GetAllLocations()
        {
            return dBContext.Locations.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetAllContainers")]
        public IList<Container> GetAllContainers()
        {
            return dBContext.Containers.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

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

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(Guid itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == itemId);
        }

        [HttpGet("GetLocationById/{locationId}")]
        public Location GetLocationById(Guid locationId)
        {
            return dBContext.Locations.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == locationId);
        }

        [HttpGet("GetContainerById/{containerId}")]
        public Container GetContainerById(Guid containerId)
        {
            return dBContext.Containers.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == containerId);
        }

        [HttpGet("GetOrderById/{orderId}")]
        public Order GetOrderById(Guid orderId)
        {
            var order = dBContext.Orders.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == orderId);
            order.OrderItems = dBContext.Items.Where(x => x.NextEventId == Guid.Empty && x.OrderId == order.Id).ToList();

            return order;
        }

        [HttpGet("GetNextOrderByStatus/{status}")]
        public Order GetNextOrderByStatus(int status)
        {
            var order = dBContext.Orders.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Status == status);
            order.OrderItems = dBContext.Items.Where(x => x.NextEventId == Guid.Empty && x.OrderId == order.Id).ToList();

            return order;
        }


        [HttpPost("PrintQRCode")]
        public async Task<StatusCodeResult> PrintQRCode(UnregisteredObject objectToRegister)
        {
            Guid objectId = Guid.NewGuid();
            objectToRegister.Id = objectId;
            string registrationString = JsonSerializer.Serialize(objectToRegister);

            controllerFunctions.printQrCodeFromRegistrationString(registrationString);

            return StatusCode(200);
        }

        [HttpPost("RegisterWarehouseObject")]
        public async Task<StatusCodeResult> RegisterWarehouseObject(UnregisteredObject objectToRegister)
        {
            switch (objectToRegister.ObjectType)
            {
                case 0:
                    RegisterItem(objectToRegister);
                    break;
                case 1:
                    RegisterLocation(objectToRegister);
                    break;
                case 2:
                    RegisterContainer(objectToRegister);
                    break;
                case 3:
                    RegisterOrder(objectToRegister);
                    break;
                default:
                    return StatusCode(500);
            }

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateItem")]
        public async Task<Item> UpdateItem(Item item)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == item.Id && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newEventId;
                Item newItem = new Item(
                    newEventId,
                    itemToUpdate.Id,
                    item.Name,
                    item.Description,
                    DateTime.Now,
                    item.Status,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    item.LocationId,
                    item.OrderId
                );

                dBContext.Entry(newItem).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }

        [HttpPost("UpdateLocation")]
        public async Task<Location> UpdateLocation(Location location)
        {
            var locationToUpdate = dBContext.Locations.FirstOrDefault(x => x.Id == location.Id && x.NextEventId == Guid.Empty);

            if (locationToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newEventId;
                Location newLocation = new Location(
                    newEventId,
                    locationToUpdate.Id,
                    location.Name,
                    location.Description,
                    DateTime.Now,
                    location.Status,
                    locationToUpdate.EventId,
                    Guid.Empty,
                    location.ItemId
                );

                dBContext.Entry(newLocation).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newLocation;
            }
            return null;
        }

        [HttpPost("UpdateContainer")]
        public async Task<Container> UpdateContainer(Container container)
        {
            var containerToUpdate = dBContext.Containers.FirstOrDefault(x => x.Id == container.Id && x.NextEventId == Guid.Empty);

            if (containerToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                containerToUpdate.NextEventId = newEventId;
                Container newContainer = new Container(
                    newEventId,
                    containerToUpdate.Id,
                    container.Name,
                    container.Description,
                    DateTime.Now,
                    container.Status,
                    containerToUpdate.EventId,
                    Guid.Empty,
                    container.ItemId
                );

                dBContext.Entry(newContainer).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newContainer;
            }
            return null;
        }

        [HttpPost("UpdateOrder")]
        public async Task<Order> UpdateOrder(Order order)
        {
            var orderToUpdate = dBContext.Orders.FirstOrDefault(x => x.Id == order.Id && x.NextEventId == Guid.Empty);

            if (orderToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                orderToUpdate.NextEventId = newEventId;
                Order newOrder = new Order(
                    newEventId,
                    orderToUpdate.Id,
                    order.Name,
                    order.Description,
                    DateTime.Now,
                    order.Status,
                    orderToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newOrder).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                newOrder.OrderItems = dBContext.Items.Where(x => x.NextEventId == Guid.Empty && x.OrderId == order.Id).ToList();

                return newOrder;
            }
            return null;
        }

        private void RegisterItem(UnregisteredObject objectToRegister)
        {
            Item newItem = new Item(
                Guid.NewGuid(),
                (Guid)objectToRegister.Id,
                objectToRegister.Name,
                objectToRegister.Description,
                DateTime.Now,
                Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION,
                Guid.Empty,
                Guid.Empty,
                Guid.Empty,
                Guid.Empty
            );
            dBContext.Items.Add(newItem);
        }

        private void RegisterLocation(UnregisteredObject objectToRegister)
        {
            Location newLocation = new Location(
                Guid.NewGuid(),
                (Guid)objectToRegister.Id,
                objectToRegister.Name,
                objectToRegister.Description,
                DateTime.Now,
                Constants.CONTAINER_EMPTY,
                Guid.Empty,
                Guid.Empty,
                Guid.Empty
            );
            dBContext.Locations.Add(newLocation);
        }

        private void RegisterContainer(UnregisteredObject objectToRegister)
        {
            Container newContainer = new Container(
                Guid.NewGuid(),
                (Guid)objectToRegister.Id,
                objectToRegister.Name,
                objectToRegister.Description,
                DateTime.Now,
                Constants.CONTAINER_EMPTY,
                Guid.Empty,
                Guid.Empty,
                Guid.Empty
            );
            dBContext.Containers.Add(newContainer);
        }

        private void RegisterOrder(UnregisteredObject objectToRegister)
        {
            Order newOrder = new Order(
                Guid.NewGuid(),
                (Guid)objectToRegister.Id,
                objectToRegister.Name,
                objectToRegister.Description,
                DateTime.Now,
                Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION,
                Guid.Empty,
                Guid.Empty
            );
            dBContext.Orders.Add(newOrder);
        }
    }
}
