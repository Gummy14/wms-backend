using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid OrderEventId { get; set; }
        public Guid OrderId { get; set; }
        public List<Item>? Items { get; set; } = new List<Item>();
        public DateTime OrderStatusDateTime { get; set; }
        public int OrderStatus { get; set; }
        public Guid PreviousOrderEventId { get; set; }
        public Guid NextOrderEventId { get; set; }

        public Order()
        {
        }
        public Order (Guid orderEventId, Guid id, List<Item> items, DateTime orderStatusDateTime, int orderStatus, Guid prevOrderEventId, Guid nextOrderEventId)
        {
            OrderEventId = orderEventId;
            OrderId = id;
            Items = items;
            OrderStatusDateTime = orderStatusDateTime;
            OrderStatus = orderStatus;
            PreviousOrderEventId = prevOrderEventId;
            NextOrderEventId = nextOrderEventId;
        }
    }
}
