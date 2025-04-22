using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Models.Orders;

namespace WMS_API.Layers.Data
{
    public class AddressRepository : IAddressRepository
    {
        private MyDbContext dBContext;

        public AddressRepository(MyDbContext context)
        {
            dBContext = context;
        }

        public async Task<Address> GetAddressByOrderIdAsync(Guid orderId)
        {
            var result = dBContext.Addresses.FirstOrDefault(x => x.OrderId == orderId);
            return result;
        }
    }
}
