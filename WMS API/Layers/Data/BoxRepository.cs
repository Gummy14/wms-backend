using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Containers;

namespace WMS_API.Layers.Data
{
    public class BoxRepository : IBoxRepository
    {
        private MyDbContext dBContext;

        public BoxRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<List<BoxData>> GetAllBoxesAsync()
        {
            var result = await dBContext.BoxData.Where(x => x.NextEventId == null).ToListAsync();
            return result;
        }

        public async Task<BoxData> GetBoxByIdAsync(Guid boxId)
        {
            var result = await dBContext.BoxData.FirstOrDefaultAsync(x => x.NextEventId == null && x.BoxId == boxId);
            return result;
        }

        public async Task<List<BoxData>> GetBoxHistoryAsync(Guid boxId)
        {
            var result = await dBContext.BoxData.Where(x => x.BoxId == boxId).ToListAsync();
            return result;
        }
        public async Task AddBoxAsync(Box box)
        {
            await dBContext.Boxes.AddAsync(box);
            await dBContext.SaveChangesAsync();
        }

        public async Task AddBoxDataAsync(BoxData boxData)
        {
            await dBContext.BoxData.AddAsync(boxData);
            await dBContext.SaveChangesAsync();
        }
    }
}
