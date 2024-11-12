using WMS_API.Models.Items;
using WMS_API.Models.Orders;

namespace WMS_API.Models
{
    public class Order
    {
        public OrderDetail OrderDetail { get; set; }
        public List<Item> Items { get; set; }

        public Order()
        {
        }
        public Order(OrderDetail orderDetail, List<Item> items)
        {
            OrderDetail = orderDetail;
            Items = items;
        }
    }
}
