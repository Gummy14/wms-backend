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

        [HttpGet("GetWarehouseObjectByStatus/{status}")]
        public WarehouseObject GetWarehouseObjectByStatus(int status)
        {
            return dBContext.WarehouseObjects.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Status == status);
        }

        [HttpGet("GetAllWarehouseRelationships")]
        public IList<WarehouseObjectRelationship> GetAllWarehouseRelationships()
        {
            return dBContext.WarehouseObjectRelationships.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetAllWarehouseRelationshipsByParentId/{parentObjectId}")]
        public IList<WarehouseObjectRelationship> GetAllWarehouseRelationshipsByParentId(Guid parentObjectId)
        {
            return dBContext.WarehouseObjectRelationships.Where(x => x.NextEventId == Guid.Empty && x.ParentId == parentObjectId).ToList();
        }

        [HttpGet("GetAllWarehouseRelationshipsByChildId/{childObjectId}")]
        public IList<WarehouseObjectRelationship> GetWarehouseParentObjectByChildObjectId(Guid childObjectId)
        {
            return dBContext.WarehouseObjectRelationships.Where(x => x.NextEventId == Guid.Empty && x.ChildId == childObjectId).ToList();
        }

        [HttpGet("GetAllWarehouseRelationshipsByParentType/{parentType}")]
        public IList<WarehouseObjectRelationship> GetAllWarehouseRelationshipsByParentType(int parentType)
        {
            var parentObjectIdsByType = dBContext.WarehouseObjects
                .Where(x => x.NextEventId == Guid.Empty && x.ObjectType == parentType)
                .GroupBy(x => x.ObjectId)
                .Select(x => x.Key)
                .ToList();

            return dBContext.WarehouseObjectRelationships.Where(x => x.NextEventId == Guid.Empty && parentObjectIdsByType.Contains(x.ParentId)).ToList();

        }

        [HttpGet("GetAllWarehouseRelationshipsByParentStatus/{status}")]
        public IList<WarehouseObjectRelationship> GetWarehouseParentObjectWithChildrenByStatus(int status)
        {
            var parentObjectIdsByStatus = dBContext.WarehouseObjects
                .Where(x => x.NextEventId == Guid.Empty && x.Status == status)
                .GroupBy(x => x.ObjectId)
                .Select(x => x.Key)
                .ToList();

            return dBContext.WarehouseObjectRelationships.Where(x => x.NextEventId == Guid.Empty && parentObjectIdsByStatus.Contains(x.ParentId)).ToList();
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
            int objectStatus = 0;

            switch (objectToRegister.ObjectType)
            {
                case 0:
                    objectStatus = Constants.ITEM_REGISTERED_WAITING_FOR_PUTAWAY_SELECTION;
                    break;
                case 1:
                    objectStatus = Constants.CONTAINER_EMPTY;
                    break;
                case 2:
                    objectStatus = Constants.CONTAINER_EMPTY;
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
                    warehouseObject.Status,
                    warehouseObjectToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newWarehouseObject).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newWarehouseObject;
            }

            return null;
        }

        [HttpPost("RegisterWarehouseObjectRelationship")]
        public async Task<WarehouseObjectRelationship> RegisterWarehouseObjectRelationship(UnregisteredObjectRelationship unregisteredObjectRelationship)
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

        [HttpPost("UpdateWarehouseObjectRelationship")]
        public async Task<WarehouseObjectRelationship> UpdateWarehouseObjectRelationship(WarehouseObjectRelationship warehouseObjectRelationship)
        {
            var warehouseObjectRelationshipToUpdate = dBContext.WarehouseObjectRelationships
                .FirstOrDefault(x => x.RelationshipId == warehouseObjectRelationship.RelationshipId && x.NextEventId == Guid.Empty);

            if (warehouseObjectRelationshipToUpdate != null)
            {
                Guid newEventId = Guid.NewGuid();
                warehouseObjectRelationshipToUpdate.NextEventId = newEventId;
                WarehouseObjectRelationship newWarehouseObjectRelationship = new WarehouseObjectRelationship(
                    newEventId,
                    warehouseObjectRelationshipToUpdate.RelationshipId,
                    warehouseObjectRelationship.ParentId,
                    warehouseObjectRelationship.ChildId,
                    DateTime.Now,
                    warehouseObjectRelationshipToUpdate.EventId,
                    Guid.Empty
                );
                dBContext.Entry(newWarehouseObjectRelationship).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newWarehouseObjectRelationship;
            }
            return null;
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
                    warehouseObject.Status,
                    warehouseObjectToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newWarehouseObject).State = EntityState.Added;
            }
        }
    }
}
