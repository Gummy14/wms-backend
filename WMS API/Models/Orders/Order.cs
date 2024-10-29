using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public DateTime DateTimeOrderRecieved { get; set; }

        public Order(Guid id, DateTime dateTimeOrderRecieved)
        {
            Id = id;
            DateTimeOrderRecieved = dateTimeOrderRecieved;
        }
        public Order (Guid id, List<OrderItem> orderItems, DateTime dateTimeOrderRecieved)
        {
            Id = id;
            OrderItems = orderItems;
            DateTimeOrderRecieved = dateTimeOrderRecieved;
        }
    }
}
