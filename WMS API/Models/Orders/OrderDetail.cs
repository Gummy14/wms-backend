using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }
        public DateTime OrderStatusDateTime { get; set; }
        public int OrderStatus { get; set; }

        public OrderDetail()
        {
        }
        public OrderDetail (Guid id, DateTime orderStatusDateTime, int orderStatus)
        {
            OrderId = id;
            OrderStatusDateTime = orderStatusDateTime;
            OrderStatus = orderStatus;
        }
    }
}
