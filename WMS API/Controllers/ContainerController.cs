using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Drawing;
using System.Text.Json;
using WMS_API.DbContexts;
using WMS_API.Models;
using ZXing.Common;
using ZXing;
using ZXing.Windows.Compatibility;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private MyDbContext dBContext;

        public ContainerController(MyDbContext context)
        {
            dBContext = context;
        }

        //[HttpGet("GetAllContainers")]
        //public IList<Container> GetAllContainers()
        //{
        //    List<Container> containers = new List<Container>();
        //    var containerDetails = dBContext.ContainerDetails.Where(x => x.NextContainerEventId == Guid.Empty).ToList();

        //    foreach (var containerDetail in containerDetails)
        //    {
        //        var items = dBContext.Items.Where(x => x.OrderId == containerDetail.ContainerId && x.NextItemEventId == Guid.Empty).ToList();
        //        containers.Add(new Container(containerDetail, items));
        //    }
        //    return containers;
        //}
    }
}
