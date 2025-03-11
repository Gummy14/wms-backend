using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private MyDbContext dBContext;
        private ControllerFunctions controllerFunctions;

        public RegistrationController(MyDbContext context)
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
            int status = 0;
            switch (objectToRegister.ObjectType)
            {
                case 0:
                    status = Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION;
                    break;
                case 1:
                    status = Constants.LOCATION_REGISTERED_AS_UNOCCUPIED;
                    break;
                case 2:
                    status = Constants.CONTAINER_REGISTERED_AS_NOT_IN_USE;
                    break;
                case 3:
                    status = Constants.ORDER_REGISTERED_WAITING_FOR_PICKING_SELECTION;
                    break;
                default:
                    break;
            }

            WarehouseObject newWarehouseObject = new WarehouseObject(
                Guid.NewGuid(),
                objectToRegister.ObjectType,
                objectToRegister.ObjectType == 0 ? (Guid)objectToRegister.Id : Guid.Empty,
                objectToRegister.ObjectType == 0 ? objectToRegister.Name : "",
                objectToRegister.ObjectType == 0 ? objectToRegister.Description : "",
                objectToRegister.ObjectType == 1 ? (Guid)objectToRegister.Id : Guid.Empty,
                objectToRegister.ObjectType == 1 ? objectToRegister.Name : "",
                objectToRegister.ObjectType == 1 ? objectToRegister.Description : "",
                objectToRegister.ObjectType == 2 ? (Guid)objectToRegister.Id : Guid.Empty,
                objectToRegister.ObjectType == 2 ? objectToRegister.Name : "",
                objectToRegister.ObjectType == 2 ? objectToRegister.Description : "",
                //Might not ever have a scenario where we register an order via QR code
                //writing scenario anyways
                objectToRegister.ObjectType == 3 ? (Guid)objectToRegister.Id : Guid.Empty,
                objectToRegister.ObjectType == 3 ? objectToRegister.Name : "",
                objectToRegister.ObjectType == 3 ? objectToRegister.Description : "",
                DateTime.Now,
                status,
                Guid.Empty,
                Guid.Empty

            );

            dBContext.WarehouseObjects.Add(newWarehouseObject);
            await dBContext.SaveChangesAsync();
        }
    }
}
