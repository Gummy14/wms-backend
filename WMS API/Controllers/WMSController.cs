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

        [HttpGet("GetAllWarehouseParentObjectsWithChildren")]
        public List<WarehouseObjectWithChildren> GetAllWarehouseParentObjectsWithChildren()
        {
            List<WarehouseObjectWithChildren> allWarehouseParentObjectsWithChildren = new List<WarehouseObjectWithChildren>();
            var parentObjectIds = dBContext.WarehouseObjectRelationships
                .Where(x => x.NextEventId == Guid.Empty)
                .GroupBy(x => x.ParentId)
                .Select(x => x.Key)
                .ToList();

            foreach (var parentObjectId in parentObjectIds) {
                allWarehouseParentObjectsWithChildren.Add(getWarehouseParentObjectWithChildrenByParentId(parentObjectId));
            }

            return allWarehouseParentObjectsWithChildren;
        }

        [HttpGet("GetWarehouseParentObjectWithChildrenByParentId/{parentObjectId}")]
        public WarehouseObjectWithChildren GetWarehouseParentObjectWithChildrenByParentId(Guid parentObjectId)
        {
            return getWarehouseParentObjectWithChildrenByParentId(parentObjectId);
        }

        [HttpGet("GetWarehouseOrderObjectWithChildrenByEventType/{eventType}")]
        public WarehouseObjectWithChildren GetWarehouseOrderObjectWithChildrenByEventType(int eventType)
        {
            var parentObject = dBContext.WarehouseObjects.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.EventType == eventType);
            return getWarehouseParentObjectWithChildrenByParentId(parentObject.ObjectId);
        }

        //[HttpGet("GetOrderByContainerId/{containerId}")]
        //public WarehouseObjectWithChildren GetOrderByContainerId(Guid containerId)
        //{
        //    var order = dBContext.WarehouseObjects.FirstOrDefault(x => x.ParentId == containerId && x.NextEventId == Guid.Empty && x.ObjectType == 3);
        //    var items = dBContext.WarehouseObjects.Where(x => x.OrderId == order.ObjectId && x.NextEventId == Guid.Empty && x.ObjectType == 0).ToList();

        //    return new WarehouseObjectWithChildren(order, items);
        //}

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
            updateWarehouseObject(warehouseObject);
            await dBContext.SaveChangesAsync();
            return null;
        }

        [HttpPost("CreateWarehouseObjectRelationship")]
        public async Task<WarehouseObjectRelationship> CreateWarehouseObjectRelationship(UnregisteredObjectRelationship unregisteredObjectRelationship)
        {
            var parentObject = dBContext.WarehouseObjects.FirstOrDefault(x => x.ObjectId == unregisteredObjectRelationship.Parent.ObjectId && x.NextEventId == Guid.Empty);
            var childObject = dBContext.WarehouseObjects.FirstOrDefault(x => x.ObjectId == unregisteredObjectRelationship.Child.ObjectId && x.NextEventId == Guid.Empty);

            if (parentObject != null && childObject != null) {
                WarehouseObjectRelationship warehouseObjectRelationship = new WarehouseObjectRelationship(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    parentObject.ObjectId,
                    childObject.ObjectId,
                    DateTime.Now,
                    Guid.Empty,
                    Guid.Empty
                );

                updateWarehouseObject(unregisteredObjectRelationship.Parent);
                updateWarehouseObject(unregisteredObjectRelationship.Child);

                dBContext.WarehouseObjectRelationships.Add(warehouseObjectRelationship);

                await dBContext.SaveChangesAsync();
            }

            return null;
        }

        //[HttpPost("DestroyWarehouseObjectRelationship")]
        //public async Task<WarehouseObjectRelationship> DestroyWarehouseObjectRelationship(WarehouseObject warehouseParentObject, WarehouseObject warehouseChildObject)
        //{
        //    var parentObject = dBContext.WarehouseObjects.FirstOrDefault(x => x.ObjectId == warehouseParentObject.ObjectId && x.NextEventId == Guid.Empty);
        //    var childObject = dBContext.WarehouseObjects.FirstOrDefault(x => x.ObjectId == warehouseChildObject.ObjectId && x.NextEventId == Guid.Empty);
        //    var currentRelationship = dBContext.WarehouseObjectRelationships.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.ParentId == parentObject.ObjectId && x.ChildId == childObject.ObjectId);


        //    dBContext.WarehouseObjectRelationships.Add(new WarehouseObjectRelationship(Guid.NewGuid(), parentObject.ObjectId, childObject.ObjectId, DateTime.Now, Guid.Empty, Guid.Empty));

        //    updateWarehouseObject(parentObject);
        //    updateWarehouseObject(childObject);

        //    await dBContext.SaveChangesAsync();

        //    return null;
        //}

        protected WarehouseObjectWithChildren getWarehouseParentObjectWithChildrenByParentId(Guid parentObjectId)
        {
            WarehouseObjectWithChildren warehouseParentObjectWithChildren = new WarehouseObjectWithChildren();

            warehouseParentObjectWithChildren.WarehouseParentObject =
                dBContext.WarehouseObjects.FirstOrDefault(x => x.ObjectId == parentObjectId && x.NextEventId == Guid.Empty);

            var childObjects = dBContext.WarehouseObjectRelationships.Where(x => x.ParentId == parentObjectId && x.NextEventId == Guid.Empty).ToList();

            foreach (var childObject in childObjects)
            {
                warehouseParentObjectWithChildren.WarehouseChildrenObjects.Add(
                    dBContext.WarehouseObjects.FirstOrDefault(x => x.ObjectId == childObject.ChildId && x.NextEventId == Guid.Empty)
                );
            }

            return warehouseParentObjectWithChildren;
        }

        protected void updateWarehouseObject(WarehouseObject warehouseObject)
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
                    DateTime.Now,
                    warehouseObject.EventType,
                    warehouseObjectToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newWarehouseObject).State = EntityState.Added;
            }
        }

        //protected void updateWarehouseRelationship(WarehouseObjectRelationship warehouseObjectRelationship)
        //{
        //    var warehouseObjectRelationshipToUpdate = dBContext.WarehouseObjectRelationships
        //        .FirstOrDefault(x => x.ObjectId == warehouseObjectRelationship.ObjectId && x.NextEventId == Guid.Empty);

        //    if (warehouseObjectToUpdate != null)
        //    {
        //        Guid newEventId = Guid.NewGuid();
        //        warehouseObjectToUpdate.NextEventId = newEventId;
        //        WarehouseObject newWarehouseObject = new WarehouseObject(
        //            newEventId,
        //            warehouseObjectToUpdate.ObjectId,
        //            warehouseObject.ObjectType,
        //            warehouseObject.Name,
        //            warehouseObject.Description,
        //            DateTime.Now,
        //            warehouseObject.EventType,
        //            warehouseObjectToUpdate.EventId,
        //            Guid.Empty
        //        );

        //        dBContext.Entry(newWarehouseObject).State = EntityState.Added;
        //    }
        //}
    }
}
