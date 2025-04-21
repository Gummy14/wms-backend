using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Models.Items;
using WMS_API.Models.Shipments;
using WMS_API.Models.Trucks;

namespace WMS_API.Layers.Data
{
    public class TruckRepository : ITruckRepository
    {
        private MyDbContext dBContext;

        public TruckRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<List<Truck>> GetAllTrucksAsync()
        {
            var result = await dBContext.Trucks.ToListAsync();
            return result;
        }

        public async Task<Truck> GetTruckByIdAsync(Guid truckId)
        {
            var result = await dBContext.Trucks.FirstOrDefaultAsync(x => x.Id == truckId);
            return result;
        }

        public async Task AddTruckAsync(Truck truck)
        {
            await dBContext.Trucks.AddAsync(truck);
            await dBContext.SaveChangesAsync();
        }

        public async Task UpdateTruckAsync(Guid truckId)
        {
            var result = await dBContext.Trucks.FirstOrDefaultAsync(x => x.Id == truckId);
            result.DepartureDateTimeStamp = DateTime.Now;
            await dBContext.SaveChangesAsync();
        }
    }
}
