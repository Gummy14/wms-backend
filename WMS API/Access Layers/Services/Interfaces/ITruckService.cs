using WMS_API.Models.Trucks;

namespace WMS_API.Layers.Services.Interfaces
{
    public interface ITruckService
    {
        Task<List<Truck>> GetAllTrucksAsync();
        Task SetTruckDepartedAsync(Guid truckId);
        Task AddBoxToTruckAsync(Guid boxId, Guid truckId);

    }
}
