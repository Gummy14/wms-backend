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

        [HttpGet("GetAllWarehouseObjects")]
        public IList<WarehouseObject> GetAllWarehouseObjects()
        {
            return dBContext.WarehouseObjects.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetAllWarehouseObjectsByType/{objectType}")]
        public IList<WarehouseObject> GetAllItems(int objectType)
        {
            return dBContext.WarehouseObjects.Where(x => x.NextEventId == Guid.Empty && x.ObjectType == objectType).ToList();
        }

        [HttpGet("GetWarehouseObjectById/{objectId}")]
        public WarehouseObject GetWarehouseObjectById(Guid objectId)
        {
            return dBContext.WarehouseObjects.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.ObjectId == objectId);
        }

        [HttpGet("GetWarehouseParentObjectWithChildrenByParentId/{parentObjectId}")]
        public WarehouseParentObjectWithChildren GetWarehouseParentObjectWithChildrenByParentId(Guid parentObjectId)
        {
            WarehouseParentObjectWithChildren warehouseParentObjectWithChildren = new WarehouseParentObjectWithChildren(
                dBContext.WarehouseObjects.FirstOrDefault(x => x.ObjectId == parentObjectId && x.NextEventId == Guid.Empty),
                dBContext.WarehouseObjects.Where(x => x.ParentId == parentObjectId && x.NextEventId == Guid.Empty).ToList()
            );

            return warehouseParentObjectWithChildren;
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

        [HttpPost("RegisterObject")]
        public async Task<StatusCodeResult> RegisterObject(UnregisteredObject objectToRegister)
        {
            int objectStatus = 0;

            switch (objectToRegister.ObjectType)
            {
                case 0:
                    objectStatus = Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION;
                    break;
                case 1:
                    objectStatus = Constants.CONTAINER_REGISTERED;
                    break;
                case 2:
                    objectStatus = Constants.CONTAINER_REGISTERED;
                    break;
                default:
                    return StatusCode(500);
            }

            WarehouseObject warehouseObject = new WarehouseObject(
                Guid.NewGuid(),
                (Guid)objectToRegister.ObjectId,
                objectToRegister.ObjectType,
                objectToRegister.Name,
                objectToRegister.Description,
                null,
                null,
                DateTime.Now,
                objectStatus,
                Guid.Empty,
                Guid.Empty
            );

            dBContext.WarehouseObjects.Add(warehouseObject);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("UpdateWarehouseObject")]
        public async Task<WarehouseObject> UpdateWarehouseObject(WarehouseObject warehouseObject)
        {
            var warehouseObjectToUpdate = dBContext.WarehouseObjects
                .FirstOrDefault(x => x.ObjectId == warehouseObject.ObjectId && x.NextEventId == Guid.Empty);

            if (warehouseObjectToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                warehouseObjectToUpdate.NextEventId = newEventId;
                WarehouseObject newWarehouseObject = new WarehouseObject(
                    newEventId,
                    warehouseObjectToUpdate.ObjectId,
                    warehouseObject.ObjectType,
                    warehouseObject.Name,
                    warehouseObject.Description,
                    warehouseObject.ParentId,
                    warehouseObject.OrderId,
                    DateTime.Now,
                    warehouseObject.EventType,
                    warehouseObjectToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newWarehouseObject).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newWarehouseObject;
            }
            return null;
        }
    }
}
