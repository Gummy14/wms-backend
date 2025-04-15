using WMS_API.Models.Boxes;
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
        public ContainerData ContainerUsedToPickOrder { get; set; }
        public BoxData BoxUsedToPackOrder { get; set; }


        public Order()
        {
        }

        public Order(
            Guid id,
            List<OrderData> orderDataHistory,
            List<ItemData>? orderItems,
            Address address,
            ContainerData containerUsedToPickOrder,
            BoxData boxUsedToPackOrder
        )
        {
            Id = id;
            OrderDataHistory = orderDataHistory;
            OrderItems = orderItems;
            Address = address;
            ContainerUsedToPickOrder = containerUsedToPickOrder;
            BoxUsedToPackOrder = boxUsedToPackOrder;
        }
    }
}
