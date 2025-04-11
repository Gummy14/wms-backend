using WMS_API.Models.Containers;
using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<OrderData> OrderDataHistory { get; set; }
        public List<ItemData>? OrderItems { get; set; }
        public Address Address { get; set; }
        public ContainerData ContainerUsedToFulfillOrder { get; set; }


        public Order()
        {
        }

        public Order(
            Guid id,
            List<OrderData> orderDataHistory,
            List<ItemData>? orderItems,
            Address address,
            ContainerData containerUsedToFulfillOrder
        )
        {
            Id = id;
            OrderDataHistory = orderDataHistory;
            OrderItems = orderItems;
            Address = address;
            ContainerUsedToFulfillOrder = containerUsedToFulfillOrder;
        }
    }
}
