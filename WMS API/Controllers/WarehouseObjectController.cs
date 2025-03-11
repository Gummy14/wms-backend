using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseObjectController : ControllerBase
    {
        private MyDbContext dBContext;

        public WarehouseObjectController(MyDbContext context)
        {
            dBContext = context;
        }

        //GET
        [HttpGet("GetAllWarehouseObjects")]
        public IList<WarehouseObject> GetAllItems()
        {
            return dBContext.WarehouseObjects.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetWarehouseObjectByIdAndType/{WarehouseObjectId}/{WarehouseObjectType}")]
        public WarehouseObject GetWarehouseObjectByIdAndType(Guid warehouseObjectId, int warehouseObjectType)
        {
            switch (warehouseObjectType)
            {
                case 0:
                    return dBContext.WarehouseObjects.FirstOrDefault(x =>
                        x.ObjectType == warehouseObjectType &&
                        x.ItemId == warehouseObjectId &&
                        x.NextEventId == Guid.Empty
                    );
                case 1:
                    return dBContext.WarehouseObjects.FirstOrDefault(x =>
                        x.ObjectType == warehouseObjectType &&
                        x.LocationId == warehouseObjectId &&
                        x.NextEventId == Guid.Empty
                    );
                case 2:
                    return dBContext.WarehouseObjects.FirstOrDefault(x =>
                        x.ObjectType == warehouseObjectType &&
                        x.ContainerId == warehouseObjectId &&
                        x.NextEventId == Guid.Empty
                    );
                case 3:
                    return dBContext.WarehouseObjects.FirstOrDefault(x =>
                        x.ObjectType == warehouseObjectType &&
                        x.OrderId == warehouseObjectId &&
                        x.NextEventId == Guid.Empty
                    );
                default:
                    return null;
            }
        }
    }
}
