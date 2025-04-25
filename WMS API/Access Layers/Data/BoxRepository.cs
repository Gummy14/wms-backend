using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Models.Boxes;

namespace WMS_API.Layers.Data
{
    public class BoxRepository : IBoxRepository
    {
        private MyDbContext dBContext;

        public BoxRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<List<Box>> GetAllBoxesMostRecentDataAsync()
        {
            var result = await dBContext.Boxes
                .Include(x => x.BoxData.Where(y => y.NextEventId == null))
                .Include(x => x.BoxItems.Where(y => y.NextEventId == null))
                .ToListAsync();

            return result;
        }

        public async Task<Box> GetBoxByIdAsync(Guid boxId)
        {
            var result = await dBContext.Boxes
                .Include(x => x.BoxData)
                .Include(x => x.BoxItems)
                .FirstOrDefaultAsync(x => x.Id == boxId);

            return result;
        }

        public async Task<BoxData> GetBoxDataByIdAsync(Guid boxId)
        {
            var result = await dBContext.BoxData
                .FirstOrDefaultAsync(x => x.NextEventId == null && x.BoxId == boxId);

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
