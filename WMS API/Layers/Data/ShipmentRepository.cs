using Microsoft.EntityFrameworkCore;
using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Models.Boxes;
using WMS_API.Models.Shipments;

namespace WMS_API.Layers.Data
{
    public class ShipmentRepository : IShipmentRepository
    {
        private MyDbContext dBContext;

        public ShipmentRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<List<ShipmentData>> GetAllShipmentsAsync()
        {
            var result = await dBContext.ShipmentData.Where(x => x.NextEventId == null).ToListAsync();
            return result;
        }

        public async Task<ShipmentData> GetShipmentByIdAsync(Guid shipmentId)
        {
            var result = await dBContext.ShipmentData.FirstOrDefaultAsync(x => x.NextEventId == null && x.ShipmentId == shipmentId);
            return result;
        }

        public async Task<List<ShipmentData>> GetShipmentHistoryAsync(Guid shipmentId)
        {
            var result = await dBContext.ShipmentData.Where(x => x.ShipmentId == shipmentId).ToListAsync();
            return result;
        }
        public async Task<ShipmentData> GetNextShipmentAsync()
        {
            var result = await dBContext.ShipmentData.FirstOrDefaultAsync(x => x.NextEventId == null);
            return result;
        }

        public async Task AddShipmentAsync(Shipment shipment)
        {
            await dBContext.Shipments.AddAsync(shipment);
            await dBContext.SaveChangesAsync();
        }

        public async Task AddShipmentDataAsync(ShipmentData shipmentData)
        {
            await dBContext.ShipmentData.AddAsync(shipmentData);
            await dBContext.SaveChangesAsync();
        }

    }
}
