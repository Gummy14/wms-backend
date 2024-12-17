using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public OrderDetail OrderDetail { get; set; }
        public List<WarehouseObject> Items { get; set; }

        public Order()
        {
        }
        public Order(OrderDetail orderDetail, List<WarehouseObject> items)
        {
            OrderDetail = orderDetail;
            Items = items;
        }
    }
}
