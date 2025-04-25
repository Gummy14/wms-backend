using WMS_API.Models.Boxes;
using WMS_API.Models.Containers;
using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<OrderData> OrderData { get; set; }
        public List<ItemData>? OrderItems { get; set; }
        public Address Address { get; set; }
        public List<ContainerData> OrderContainer { get; set; }
        public List<BoxData> OrderBox { get; set; }


        public Order()
        {
        }

        public Order(
            Guid id,
            List<OrderData> orderData,
            List<ItemData>? orderItems,
            Address address,
            List<ContainerData> orderContainer,
            List<BoxData> orderBox
        )
        {
            Id = id;
            OrderData = orderData;
            OrderItems = orderItems;
            Address = address;
            OrderContainer = orderContainer;
            OrderBox = orderBox;
        }
    }
}
