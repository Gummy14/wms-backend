using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using WMS_API.DbContexts;
using WMS_API.Migrations;
using WMS_API.Models;
using WMS_API.Models.Events;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using WMS_API.Models.WarehouseObjects;

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

        [HttpGet("GetAllOrders")]
        public IList<Order> GetAllOrders()
        {
            return dBContext.Orders.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(Guid itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.ObjectId == itemId);
        }

        [HttpGet("GetLocationById/{locationId}")]
        public Location GetLocationById(Guid locationId)
        {
            return dBContext.Locations.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.ObjectId == locationId);
        }

        [HttpGet("GetOrderById/{orderId}")]
        public Order GetOrderById(Guid orderId)
        {
            return dBContext.Orders.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.ObjectId == orderId);
        }

        [HttpGet("GetNextOrderByStatus/{status}")]
        public Order GetNextOrderByStatus(int status)
        {
            return dBContext.Orders.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Status == status);
        }


        [HttpPost("PrintQRCode")]
        public async Task<StatusCodeResult> PrintQRCode(UnregisteredObject objectToRegister)
        {
            Guid objectId = Guid.NewGuid();
            objectToRegister.ObjectId = objectId;
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
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.ObjectId == item.ObjectId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newEventId;
                Item newItem = new Item(
                    newEventId,
                    itemToUpdate.ObjectId,
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
            var locationToUpdate = dBContext.Locations.FirstOrDefault(x => x.ObjectId == location.ObjectId && x.NextEventId == Guid.Empty);

            if (locationToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newEventId;
                Location newLocation = new Location(
                    newEventId,
                    locationToUpdate.ObjectId,
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

        [HttpPost("UpdateOrder")]
        public async Task<Order> UpdateOrder(Order order)
        {
            var orderToUpdate = dBContext.Orders.FirstOrDefault(x => x.ObjectId == order.ObjectId && x.NextEventId == Guid.Empty);

            if (orderToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                orderToUpdate.NextEventId = newEventId;
                Order newOrder = new Order(
                    newEventId,
                    orderToUpdate.ObjectId,
                    order.Name,
                    order.Description,
                    DateTime.Now,
                    order.Status,
                    orderToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newOrder).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newOrder;
            }
            return null;
        }

        private void RegisterItem(UnregisteredObject objectToRegister)
        {
            Item newItem = new Item(
                Guid.NewGuid(),
                (Guid)objectToRegister.ObjectId,
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
                (Guid)objectToRegister.ObjectId,
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

        private void RegisterOrder(UnregisteredObject objectToRegister)
        {
            Order newOrder = new Order(
                Guid.NewGuid(),
                (Guid)objectToRegister.ObjectId,
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
