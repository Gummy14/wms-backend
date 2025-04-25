using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Models.Containers;

namespace WMS_API.Layers.Data
{
    public class ContainerRepository : IContainerRepository
    {
        private MyDbContext dBContext;

        public ContainerRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<List<Container>> GetAllContainersMostRecentDataAsync()
        {
            var result = await dBContext.Containers
                .Include(x => x.ContainerData.Where(y => y.NextEventId == null))
                .Include(x => x.ContainerItems.Where(y => y.NextEventId == null))
                .ToListAsync();

            return result;
        }

        public async Task<Container> GetContainerByIdAsync(Guid containerId)
        {
            var result = await dBContext.Containers
                .Include(x => x.ContainerData)
                .Include(x => x.ContainerItems)
                .FirstOrDefaultAsync(x => x.Id == containerId);
            
            return result;
        }

        public async Task<ContainerData> GetContainerDataByIdAsync(Guid containerId)
        {
            var result = await dBContext.ContainerData
                .FirstOrDefaultAsync(x => x.NextEventId == null && x.ContainerId == containerId);

            return result;
        }

        public async Task AddContainerAsync(Container container)
        {
            await dBContext.Containers.AddAsync(container);
            await dBContext.SaveChangesAsync();
        }

        public async Task AddContainerDataAsync(ContainerData containerData)
        {
            await dBContext.ContainerData.AddAsync(containerData);
            await dBContext.SaveChangesAsync();
        }
    }
}
