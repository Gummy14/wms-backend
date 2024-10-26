namespace WMS_API.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<Item> OrderItems { get; set; }
        public DateTime DateTimeOrderRecieved { get; set; }
        public DateTime DateTimeOrderFulfilled { get; set; }
    }
}
