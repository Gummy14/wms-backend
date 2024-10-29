using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }

        public OrderItem() 
        {
        }

        public OrderItem(Item item)
        {
            Item = item;
        }
    }
}
