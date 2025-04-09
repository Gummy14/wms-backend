using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class UnregisteredOrder
    {
        public List<ItemData> OrderItems { get; set; }
        public Address Address { get; set; }
    }
}
