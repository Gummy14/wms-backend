using WMS_API.Models.Trucks;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface ITruckService
    {
        Task<List<Truck>> GetAllTrucksAsync();
        Task RegisterTruckAsync(string licensePlate);
        Task AddShipmentToTruckAsync(Guid shipmentId, Guid truckId);
        Task SetTruckDepartedAsync(Guid truckId);

    }
}
