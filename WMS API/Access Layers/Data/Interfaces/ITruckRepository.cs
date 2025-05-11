using WMS_API.Models.Trucks;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface ITruckRepository
    {
        Task<List<Truck>> GetAllTrucksAsync();
        Task<Truck> GetTruckByIdAsync(Guid truckId);
        Task AddTruckAsync(Truck truck);
        Task UpdateTruckAsync(Guid truckId);
    }
}
