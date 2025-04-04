﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Models;
using WMS_API.Models.Boxes;
using WMS_API.Models.Items;
using WMS_API.Models.Locations;
using Container = WMS_API.Models.Containers.Container;

namespace WMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private MyDbContext dBContext;

        public ItemController(MyDbContext context)
        {
            dBContext = context;
        }

        //GET
        [HttpGet("GetAllItems")]
        public IList<Item> GetAllItems()
        {
            return dBContext.Items.Where(x => x.NextEventId == Guid.Empty).ToList();
        }

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(Guid itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.NextEventId == Guid.Empty && x.Id == itemId);
        }

        [HttpGet("GetItemHistory/{itemId}")]
        public List<Item> GetItemHistory(Guid itemId)
        {
            List<Item> itemHistory = new List<Item>();
            var allItemEvents = dBContext.Items.Where(x => x.Id == itemId);
            var firstItemEvent = allItemEvents.FirstOrDefault(x => x.PreviousEventId == Guid.Empty);
            itemHistory.Add(firstItemEvent);

            while (itemHistory.LastOrDefault().NextEventId != Guid.Empty)
            {
                var nextEvent = allItemEvents.FirstOrDefault(x => x.EventId == itemHistory.LastOrDefault().NextEventId);
                itemHistory.Add(nextEvent);
            }
            return itemHistory;
        }

        //POST
        [HttpPost("PutawayItem/{itemId}/{locationId}")]
        public async Task<Item> PutawayItem(Guid itemId, Guid locationId)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == itemId && x.NextEventId == Guid.Empty);
            var locationToUpdate = dBContext.Locations.FirstOrDefault(x => x.Id == locationId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null && locationToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newItemEventId;
                Item newItem = new Item(
                    newItemEventId,
                    itemToUpdate.Id,
                    itemToUpdate.Name,
                    itemToUpdate.Description,
                    dateTimeNow,
                    Constants.ITEM_PUTAWAY_INTO_LOCATION_COMPLETE,
                    itemToUpdate.LengthInCentimeters,
                    itemToUpdate.WidthInCentimeters,
                    itemToUpdate.HeightInCentimeters,
                    itemToUpdate.WeightInKilograms,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    locationToUpdate.Id,
                    locationToUpdate.Name,
                    itemToUpdate.ContainerId,
                    itemToUpdate.ContainerName,
                    itemToUpdate.OrderId,
                    itemToUpdate.OrderName,
                    itemToUpdate.BoxId,
                    itemToUpdate.BoxName
                );

                Guid newLocationEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newLocationEventId;
                Location newLocation = new Location(
                    newLocationEventId,
                    locationToUpdate.Id,
                    locationToUpdate.Name,
                    locationToUpdate.Description,
                    dateTimeNow,
                    Constants.LOCATION_OCCUPIED,
                    locationToUpdate.LengthInCentimeters,
                    locationToUpdate.WidthInCentimeters,
                    locationToUpdate.HeightInCentimeters,
                    locationToUpdate.MaxWeightInKilograms,
                    locationToUpdate.EventId,
                    Guid.Empty,
                    itemToUpdate.Id,
                    itemToUpdate.Name
                );

                dBContext.Entry(newItem).State = EntityState.Added;
                dBContext.Entry(newLocation).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }

        [HttpPost("PickItem/{itemId}/{containerId}")]
        public async Task<Item> PickItem(Guid itemId, Guid containerId)
        {
            var itemToUpdate = dBContext.Items.FirstOrDefault(x => x.Id == itemId && x.NextEventId == Guid.Empty);
            var locationToUpdate = dBContext.Locations.FirstOrDefault(x => x.Id == itemToUpdate.LocationId && x.NextEventId == Guid.Empty);
            var containerToUpdate = dBContext.Containers.FirstOrDefault(x => x.Id == containerId && x.NextEventId == Guid.Empty);

            if (itemToUpdate != null && locationToUpdate != null && containerToUpdate != null)
            {
                var dateTimeNow = DateTime.Now;

                Guid newItemEventId = Guid.NewGuid();
                itemToUpdate.NextEventId = newItemEventId;
                Item newItem = new Item(
                    newItemEventId,
                    itemToUpdate.Id,
                    itemToUpdate.Name,
                    itemToUpdate.Description,
                    dateTimeNow,
                    Constants.ITEM_PICKED_INTO_CONTAINER,
                    itemToUpdate.LengthInCentimeters,
                    itemToUpdate.WidthInCentimeters,
                    itemToUpdate.HeightInCentimeters,
                    itemToUpdate.WeightInKilograms,
                    itemToUpdate.EventId,
                    Guid.Empty,
                    Guid.Empty,
                    "",
                    containerToUpdate.Id,
                    containerToUpdate.Name,
                    itemToUpdate.OrderId,
                    itemToUpdate.OrderName,
                    itemToUpdate.BoxId,
                    itemToUpdate.BoxName
                );

                Guid newLocationEventId = Guid.NewGuid();
                locationToUpdate.NextEventId = newLocationEventId;
                Location newLocation = new Location(
                    newLocationEventId,
                    locationToUpdate.Id,
                    locationToUpdate.Name,
                    locationToUpdate.Description,
                    dateTimeNow,
                    Constants.LOCATION_UNOCCUPIED,
                    locationToUpdate.LengthInCentimeters,
                    locationToUpdate.WidthInCentimeters,
                    locationToUpdate.HeightInCentimeters,
                    locationToUpdate.MaxWeightInKilograms,
                    locationToUpdate.EventId,
                    Guid.Empty,
                    Guid.Empty,
                    ""
                );

                Guid newContainerEventId = Guid.NewGuid();
                containerToUpdate.NextEventId = newContainerEventId;
                Container newContainer = new Container(
                    newContainerEventId,
                    containerToUpdate.Id,
                    containerToUpdate.Name,
                    containerToUpdate.Description,
                    dateTimeNow,
                    Constants.CONTAINER_IN_USE,
                    containerToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newItem).State = EntityState.Added;
                dBContext.Entry(newLocation).State = EntityState.Added;
                dBContext.Entry(newContainer).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return newItem;
            }
            return null;
        }
        
        [HttpPost("PackItems/{containerId}/{boxId}")]
        public async Task<Box> PackItems(Guid containerId, Guid boxId)
        {
            var itemsToUpdate = dBContext.Items.Where(x => x.ContainerId == containerId).ToList();
            var containerToUpdate = dBContext.Containers.FirstOrDefault(x => x.Id == containerId);
            var boxToUpdate = dBContext.Boxes.FirstOrDefault(x => x.Id == boxId);

            if (boxToUpdate != null && containerToUpdate != null) 
            {
                var dateTimeNow = DateTime.Now;

                foreach (var item in itemsToUpdate)
                {
                    Guid newItemEventId = Guid.NewGuid();
                    item.NextEventId = newItemEventId;
                    Item newItem = new Item(
                        newItemEventId,
                        item.Id,
                        item.Name,
                        item.Description,
                        dateTimeNow,
                        Constants.ITEM_PACKED_IN_BOX,
                        item.LengthInCentimeters,
                        item.WidthInCentimeters,
                        item.HeightInCentimeters,
                        item.WeightInKilograms,
                        item.EventId,
                        Guid.Empty,
                        item.LocationId,
                        item.LocationName,
                        Guid.Empty,
                        "",
                        item.OrderId,
                        item.OrderName,
                        boxToUpdate.Id,
                        boxToUpdate.Name

                    );

                    dBContext.Entry(newItem).State = EntityState.Added;
                }

                Guid newContainerEventId = Guid.NewGuid();
                containerToUpdate.NextEventId = newContainerEventId;
                Container newContainer = new Container(
                    newContainerEventId,
                    containerToUpdate.Id,
                    containerToUpdate.Name,
                    containerToUpdate.Description,
                    dateTimeNow,
                    Constants.CONTAINER_NOT_IN_USE,
                    containerToUpdate.EventId,
                    Guid.Empty
                );

                dBContext.Entry(newContainer).State = EntityState.Added;

                await dBContext.SaveChangesAsync();

                return boxToUpdate;
            }

            return null;
        }
    }
}
