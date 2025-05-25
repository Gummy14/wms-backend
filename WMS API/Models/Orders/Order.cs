using WMS_API.Models.Boxes;
using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<OrderData> OrderData { get; set; }
        public List<ItemData>? OrderItems { get; set; }
        public Address Address { get; set; }
        public List<BoxData> OrderBox { get; set; }


        public Order()
        {
        }

        public Order(
            Guid id,
            List<OrderData> orderData,
            List<ItemData>? orderItems,
            Address address,
            List<BoxData> orderBox
        )
        {
            Id = id;
            OrderData = orderData;
            OrderItems = orderItems;
            Address = address;
            OrderBox = orderBox;
        }
    }
}
