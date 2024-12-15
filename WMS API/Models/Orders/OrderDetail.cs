namespace WMS_API.Models.Orders
{
    public class OrderDetail
    {
        public Guid OrderEventId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime OrderStatusDateTime { get; set; }
        public int OrderStatus { get; set; }
        public Guid ContainerIdOrderItemsHeldIn { get; set; }
        public Guid PreviousOrderEventId { get; set; }
        public Guid NextOrderEventId { get; set; }

        public OrderDetail()
        {
        }
        public OrderDetail (Guid orderEventId, Guid id, DateTime orderStatusDateTime, int orderStatus, Guid containerIdOrderItemsHeldIn, Guid prevOrderEventId, Guid nextOrderEventId)
        {
            OrderEventId = orderEventId;
            OrderId = id;
            OrderStatusDateTime = orderStatusDateTime;
            OrderStatus = orderStatus;
            ContainerIdOrderItemsHeldIn = containerIdOrderItemsHeldIn;
            PreviousOrderEventId = prevOrderEventId;
            NextOrderEventId = nextOrderEventId;
        }
    }
}
