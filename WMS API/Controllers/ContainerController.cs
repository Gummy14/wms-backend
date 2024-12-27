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
    }
}
