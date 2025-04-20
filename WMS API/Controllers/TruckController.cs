using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Boxes;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using WMS_API.Models.Orders;
using WMS_API.Models.Trucks;
using WMS_API.Models.WarehouseObjects;
using ContainerData = WMS_API.Models.Containers.ContainerData;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TruckController : ControllerBase
    {
        private MyDbContext dBContext;

        public TruckController(MyDbContext context)
        {
            dBContext = context;
        }

        //GET
        [HttpGet("GetAllTrucks")]
        public IList<Truck> GetAllTrucks()
        {
            return dBContext.Trucks.ToList();
        }

    }
}
