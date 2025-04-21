using WMS_API.Models.Shipments;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IShipmentService
    {
        Task<List<ShipmentData>> GetAllShipmentsAsync();
        Task<ShipmentData> GetShipmentByIdAsync(Guid shipmentId);
        Task<List<ShipmentData>> GetShipmentHistoryAsync(Guid shipmentId);
        Task RegisterShipmentAsync(UnregisteredObject objectToRegister);
        Task AddShipmentToTruckAsync(Guid shipmentId, string truckLicensePlate);
    }
}
