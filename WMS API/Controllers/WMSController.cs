using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using WMS_API.DbContexts;
using WMS_API.Migrations;
using WMS_API.Models;
using WMS_API.Models.Containers;
using WMS_API.Models.Events;
using WMS_API.Models.Items;
using WMS_API.Models.Orders;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WMSController : ControllerBase
    {
        private MyDbContext dBContext;

        public WMSController(MyDbContext context)
        {
            dBContext = context;
        }

        [HttpGet("GetItemContainerRelationship/{genericId}")]
        public ItemContainer GetItemContainerRelationship(Guid genericId)
        {
            ItemContainer itemContainer = new ItemContainer();
            var container = dBContext.Containers
                .FirstOrDefault(
                x => x.ContainerId == genericId &&
                x.NextContainerEventId == Guid.Empty);

            if (container != null)
            {
                var item = dBContext.Items
                    .FirstOrDefault(
                    x => x.ItemId == container.ItemId &&
                    x.NextItemEventId == Guid.Empty);

                itemContainer.Item = item;
                itemContainer.Container = container;

                return itemContainer;
            }
            else
            {
                var item = dBContext.Items
                    .FirstOrDefault(
                    x => x.ItemId == genericId &&
                    x.NextItemEventId == Guid.Empty);

                var newContainer = dBContext.Containers
                    .FirstOrDefault(
                    x => x.ItemId == item.ItemId &&
                    x.NextContainerEventId == Guid.Empty);

                itemContainer.Item = item;
                itemContainer.Container = newContainer;

                return itemContainer;
            }
        }
    }
}
