using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Controllers;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Models.Items;

namespace WMS_API.Layers.Data
{
    public class ItemRepository : IItemRepository
    {
        private MyDbContext dBContext;

        public ItemRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<List<Item>> GetAllItemsMostRecentDataAsync()
        {
            var result = await dBContext.Items
                .Include(x => x.ItemData.Where(y => y.NextEventId == null))
                .ToListAsync();

            return result;
        }

        public async Task<Item> GetItemByIdAsync(Guid itemId)
        {
            var result = await dBContext.Items
                .Include(x => x.ItemData.Where(y => y.NextEventId == null))
                .FirstOrDefaultAsync(x => x.Id == itemId);

            return result;
        }

        public async Task<List<ItemData>> GetItemHistoryByIdAsync(Guid itemId)
        {
            var result = await dBContext.ItemData
                .Where(x => x.ItemId == itemId)
                .ToListAsync();

            return result;
        }

        public async Task<ItemData> GetItemDataByIdAsync(Guid itemId)
        {
            var result = await dBContext.ItemData
                .FirstOrDefaultAsync(x => x.NextEventId == null && x.ItemId == itemId);

            return result;
        }

        public async Task<List<ItemData>> GetAllItemsInOrderAsync(List<ItemData> itemsInOrder)
        {
            var result = await dBContext.ItemData
                .Where(x => itemsInOrder.Contains(x)).ToListAsync();

            return result;
        }

        public async Task AddItemAsync(Item item)
        {
            await dBContext.Items.AddAsync(item);
            await dBContext.SaveChangesAsync();
        }
        public async Task AddItemDataAsync(ItemData itemData)
        {
            await dBContext.ItemData.AddAsync(itemData);
            await dBContext.SaveChangesAsync();
        }
    }
}
