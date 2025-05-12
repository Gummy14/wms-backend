using WMS_API.Models.Boxes;
using WMS_API.Models.Shipments;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IShipmentRepository
    {
        Task<List<Shipment>> GetAllShipmentsMostRecentDataAsync();
        Task<Shipment> GetShipmentByIdAsync(Guid shipmentId);
        Task<List<ShipmentData>> GetShipmentHistoryByIdAsync(Guid shipmentId);
        Task<ShipmentData> GetShipmentDataByIdAsync(Guid shipmentId);
        Task<ShipmentData> GetNextShipmentAsync();
        Task AddShipmentAsync(Shipment shipment);
        Task AddShipmentDataAsync(ShipmentData shipmentData);
    }
}
