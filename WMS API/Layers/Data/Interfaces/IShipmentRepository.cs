using WMS_API.Models.Boxes;
using WMS_API.Models.Shipments;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IShipmentRepository
    {
        Task<List<ShipmentData>> GetAllShipmentsAsync();
        Task<ShipmentData> GetShipmentByIdAsync(Guid shipmentId);
        Task<List<ShipmentData>> GetShipmentHistoryAsync(Guid shipmentId);
        Task<ShipmentData> GetNextShipmentAsync();
        Task AddShipmentAsync(Shipment shipment);
        Task AddShipmentDataAsync(ShipmentData shipmentData);
    }
}
