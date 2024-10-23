using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WMS_API.DbContexts;
using WMS_API.Models;

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

        [HttpGet("GetAllItems")]
        public IList<Item> GetAllItems()
        {
            return dBContext.Items.ToList();
        }

        [HttpGet("GetAllRegisteredItems")]
        public IList<Item> GetAllRegisteredItems()
        {
            return dBContext.Items.ToList();
        }

        [HttpGet("GetItemById/{itemId}")]
        public Item GetItemById(int itemId)
        {
            return dBContext.Items.FirstOrDefault(x => x.Id == itemId);
        }

        [HttpGet("GetPutawayLocation")]
        public Container GetPutawayLocation()
        {
            List<Container> allContainers = new List<Container>(dBContext.Containers.ToList());
            List<ItemContainerEvent> containersWithEvents = new List<ItemContainerEvent>(dBContext.ItemContainerEvents.ToList());
            List<bool> availableContainers = Enumerable.Repeat(true, allContainers.Count).ToList();

            foreach (var itemContainerEvent in containersWithEvents) {
                int index = allContainers.FindIndex(x => x.Id == itemContainerEvent.ContainerId);
                if (itemContainerEvent.EventType == 1)
                {
                    availableContainers[index] = false;
                }
                else if (itemContainerEvent.EventType == 2 && availableContainers[index] == false)
                {
                    availableContainers[index] = true;
                }

            }

            int firstAvailableContainer = availableContainers.FindIndex(x => x == true);

            return allContainers[firstAvailableContainer];
        }

        [HttpPost("RegisterItem")]
        public async Task<StatusCodeResult> RegisterItem(ItemToRegister itemToRegister)
        {
            Item item = new Item(itemToRegister.Name, itemToRegister.Description);

            dBContext.Items.Add(item);
           
            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("RegisterContainer")]
        public async Task<StatusCodeResult> RegisterContainer(int containerToRegister)
        {
            Container container = new Container(containerToRegister);

            dBContext.Containers.Add(container);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PutawayItem")]
        public async Task<StatusCodeResult> PutawayItem(ItemContainerEventToRegister itemContainerEventToRegister)
        {
            ItemContainerEvent itemContainerEvent = new ItemContainerEvent(itemContainerEventToRegister.ItemId, itemContainerEventToRegister.ContainerId, 1, DateTime.Now);

            dBContext.ItemContainerEvents.Add(itemContainerEvent);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }

        [HttpPost("PickItem")]
        public async Task<StatusCodeResult> PickItem(Item item)
        {
            var data = dBContext.Items.FirstOrDefault(x => x.Id == item.Id);

            await dBContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
