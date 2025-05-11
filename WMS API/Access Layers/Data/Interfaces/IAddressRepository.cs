using WMS_API.Models.Orders;
using WMS_API.Models.Shipments;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressByOrderIdAsync(Guid orderId);
    }
}
