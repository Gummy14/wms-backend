using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<Item>? OrderItems { get; set; }
        public DateTime DateTimeOrderRecieved { get; set; }
        public DateTime DateTimeOrderFulfilled { get; set; }

        public Order (Guid id, DateTime dateTimeOrderRecieved)
        {
            Id = id;
            DateTimeOrderRecieved = dateTimeOrderRecieved;
        }
    }
}
