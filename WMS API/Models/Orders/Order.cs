using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<OrderData> OrderDataHistory { get; set; }
        public List<ItemData>? OrderItems { get; set; }
        public Address Address { get; set; }


        public Order()
        {
        }

        public Order(
            Guid id,
            List<OrderData> orderDataHistory,
            List<ItemData>? orderItems,
            Address address
        )
        {
            Id = id;
            OrderDataHistory = orderDataHistory;
            OrderItems = orderItems;
            Address = address;
        }
    }
}
