using WMS_API.Models.Shipments;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface IShipmentService
    {
        Task<List<Shipment>> GetAllShipmentsMostRecentDataAsync();
        Task<Shipment> GetShipmentByIdAsync(Guid shipmentId);
        Task<List<ShipmentData>> GetShipmentHistoryByIdAsync(Guid shipmentId);
        Task RegisterShipmentAsync(UnregisteredObject objectToRegister);
        Task AddBoxToShipmentAsync(Guid boxId);
    }
}
