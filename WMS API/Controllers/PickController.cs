using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PickController : ControllerBase
    {
        private MyDbContext dBContext;

        public PickController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpPost("PickItem")]
        public async Task<WarehouseObject> PickItem(WarehouseObject warehouseObject)
        {
            var warehouseObjectToPick = dBContext.WarehouseObjects.FirstOrDefault(
                x => x.ObjectId == warehouseObject.ObjectId && 
                x.NextEventId == Guid.Empty);

            if (warehouseObjectToPick != null)
            {
                Guid pickBeforeEventId = Guid.NewGuid();
                Guid pickAfterEventId = Guid.NewGuid();
                DateTime dateTimeNow = DateTime.Now;

                warehouseObjectToPick.NextEventId = pickBeforeEventId;

                WarehouseObject pickBeforeWarehouseObject = new WarehouseObject(
                    pickBeforeEventId,
                    warehouseObjectToPick.ObjectId,
                    warehouseObjectToPick.ObjectType,
                    warehouseObjectToPick.Name,
                    warehouseObjectToPick.Description,
                    warehouseObjectToPick.ParentId,
                    warehouseObjectToPick.OrderId,
                    dateTimeNow,
                    Constants.ITEM_PICKED_FROM_CONTAINER_BEFORE,
                    warehouseObjectToPick.EventId,
                    pickAfterEventId
                );
                WarehouseObject pickAfterWarehouseObject = new WarehouseObject(
                    pickAfterEventId,
                    warehouseObjectToPick.ObjectId,
                    warehouseObjectToPick.ObjectType,
                    warehouseObjectToPick.Name,
                    warehouseObjectToPick.Description,
                    warehouseObject.ParentId,
                    warehouseObjectToPick.OrderId,
                    dateTimeNow,
                    Constants.ITEM_PICKED_FROM_CONTAINER_AFTER,
                    pickAfterEventId,
                    Guid.Empty
                );

                dBContext.Entry(pickBeforeWarehouseObject).State = EntityState.Added;
                dBContext.Entry(pickAfterWarehouseObject).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return pickAfterWarehouseObject;
            };

            return null;
        }
    }
}
