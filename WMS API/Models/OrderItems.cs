using WMS_API.Models.Items;
using WMS_API.Models.Orders;

namespace WMS_API.Models
{
    public class OrderItems
    {
        public Order Order { get; set; }
        public List<Item> Items { get; set; }

        public OrderItems() 
        {
        }
        public OrderItems(Order order, List<Item> items) 
        {
            Order = order;
            Items = items;
        }
    }
}
