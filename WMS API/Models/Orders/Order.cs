using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<Item>? Items { get; set; } = new List<Item>();
        public DateTime DateTimeOrderRecieved { get; set; }

        public Order(Guid id, DateTime dateTimeOrderRecieved)
        {
            Id = id;
            DateTimeOrderRecieved = dateTimeOrderRecieved;
        }
        public Order (Guid id, List<Item> items, DateTime dateTimeOrderRecieved)
        {
            Id = id;
            Items = items;
            DateTimeOrderRecieved = dateTimeOrderRecieved;
        }
    }
}
