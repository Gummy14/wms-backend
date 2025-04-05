using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Boxes;
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

        [HttpPost("PrintQRCode")]
        public async Task<StatusCodeResult> PrintQRCode(UnregisteredObject objectToRegister)
        {
            Guid objectId = Guid.NewGuid();
            objectToRegister.Id = objectId;

            await RegisterWarehouseObject(objectToRegister);

            string registrationString = JsonSerializer.Serialize(objectToRegister);
            controllerFunctions.printQrCodeFromRegistrationString(registrationString);

            return StatusCode(200);
        }

        [HttpPost("RegisterQRCode")]
        public async Task<StatusCodeResult> RegisterQRCode(UnregisteredObject objectToRegister)
        {
            await RegisterWarehouseObject(objectToRegister);

            return StatusCode(200);
        }

        private async Task RegisterWarehouseObject(UnregisteredObject objectToRegister)
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
                case 4:
                    RegisterBox(objectToRegister);
                    break;
                default:
                    break;
            }

            await dBContext.SaveChangesAsync();
        }

        private void RegisterItem(UnregisteredObject objectToRegister)
        {
            Item newItem = new Item(
                Guid.NewGuid(),
                (Guid)objectToRegister.Id,
                objectToRegister.Name,
                objectToRegister.Description,
                DateTime.Now,
                Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY,
                Guid.Empty,
                Guid.Empty,
                Guid.Empty,
                "",
                Guid.Empty,
                "",
                Guid.Empty,
                "",
                Guid.Empty,
                ""
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
                Guid.Empty,
                ""
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
                Constants.ORDER_REGISTERED_WAITING_FOR_ACKNOWLEDGEMENT,
                Guid.Empty,
                Guid.Empty
            );
            dBContext.Orders.Add(newOrder);
        }

        private void RegisterBox(UnregisteredObject objectToRegister)
        {
            Box newBox = new Box(
                Guid.NewGuid(),
                (Guid)objectToRegister.Id,
                objectToRegister.Name,
                objectToRegister.Description,
                DateTime.Now,
                Constants.BOX_REGISTERED,
                Guid.Empty,
                Guid.Empty,
                objectToRegister.Length,
                objectToRegister.Width,
                objectToRegister.Height
            );
            dBContext.Boxes.Add(newBox);
        }
    }
}
