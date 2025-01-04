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

        [HttpGet("GetNextOrderWaitingForPicking")]
        public Order GetNextOrderWaitingForPicking()
        {
            var order = dBContext.Orders.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Status == Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION);
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

        [HttpPost("UpdateItemSelectForPutaway/{itemId}")]
        public async Task<Item> UpdateItemSelectForPutaway(Guid itemId)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == itemId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newEventId;
                Item newItem = new Item(
                    newEventId,
                    itemToUpdate.Id,
                    itemToUpdate.Name,
                    itemToUpdate.Description,
                    DateTime.Now,
                    Constants.ITEM_SELECTED_FOR_PUTAWAY_PUTAWAY_IN_PROGRESS,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    itemToUpdate.LocationId,
                    itemToUpdate.ContainerId,
                    itemToUpdate.OrderId
                );

                dBContext.Entry(newItem).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }

        [HttpPost("UpdateItemPutInLocation/{itemId}/{locationId}")]
        public async Task<Item> UpdateItemPutInLocation(Guid itemId, Guid locationId)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == itemId && x.NextEventId == Guid.Empty);
            var locationToUpdate = dBContext.Locations.FirstOrDefault(x => x.Id == locationId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null && locationToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newItemEventId;
                Item newItem = new Item(
                    newItemEventId,
                    itemToUpdate.Id,
                    itemToUpdate.Name,
                    itemToUpdate.Description,
                    dateTimeNow,
                    Constants.ITEM_PUTAWAY_INTO_LOCATION_COMPLETE,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    locationId,
                    itemToUpdate.ContainerId,
                    itemToUpdate.OrderId
                );

                Guid newLocationEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newLocationEventId;
                Location newLocation = new Location(
                    newLocationEventId,
                    locationToUpdate.Id,
                    locationToUpdate.Name,
                    locationToUpdate.Description,
                    dateTimeNow,
                    Constants.LOCATION_OCCUPIED,
                    locationToUpdate.EventId,
                    Guid.Empty,
                    itemToUpdate.Id
                );

                dBContext.Entry(newItem).State = EntityState.Added;
                dBContext.Entry(newLocation).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }

        [HttpPost("UpdateItemPick/{itemId}/{containerId}")]
        public async Task<Item> UpdateItemPick(Guid itemId, Guid containerId)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == itemId && x.NextEventId == Guid.Empty);
            var locationToUpdate = dBContext.Locations.FirstOrDefault(x => x.Id == itemToUpdate.LocationId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null && locationToUpdate != null && locationToUpdate.ItemId != Guid.Empty)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newItemEventId;
                Item newItem = new Item(
                    newItemEventId,
                    itemToUpdate.Id,
                    itemToUpdate.Name,
                    itemToUpdate.Description,
                    dateTimeNow,
                    Constants.ITEM_PICKED_INTO_CONTAINER,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    Guid.Empty,
                    containerId,
                    itemToUpdate.OrderId
                );

                Guid newLocationEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newLocationEventId;
                Location newLocation = new Location(
                    newLocationEventId,
                    locationToUpdate.Id,
                    locationToUpdate.Name,
                    locationToUpdate.Description,
                    dateTimeNow,
                    Constants.LOCATION_UNOCCUPIED,
                    locationToUpdate.EventId,
                    Guid.Empty,
                    Guid.Empty
                );

                dBContext.Entry(newItem).State = EntityState.Added;
                dBContext.Entry(newLocation).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
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
                Constants.LOCATION_REGISTERED_AS_UNOCCUPIED,
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
                Constants.CONTAINER_REGISTERED_AS_NOT_IN_USE,
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
