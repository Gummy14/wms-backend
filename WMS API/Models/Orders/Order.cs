using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class Order
    {
        public Guid OrderEventId { get; set; }
        public Guid OrderId { get; set; }
        //public List<Item>? Items { get; set; } = new List<Item>();
        public int EventType { get; set; }
        public int NumberOfItemsPickedForOrder { get; set; }
        public int TotalNumberOfItemsInOrder { get; set; }
        public DateTime EventDateTime { get; set; }
        public Guid PreviousOrderEventId { get; set; }
        public Guid NextOrderEventId { get; set; }

        public Order()
        {
        }
        public Order (
            Guid orderEventId,
            Guid orderId,
            //List<Item> items,
            int eventType,
            DateTime eventDateTime,
            int numberOfItemsPickedForOrder, 
            int totalNumberOfItemsInOrder,
            Guid previousOrderEventId,
            Guid nextOrderEventId
        )
        {
            OrderEventId = orderEventId;
            OrderId = orderId;
            //Items = items;
            EventType = eventType;
            EventDateTime = eventDateTime;
            NumberOfItemsPickedForOrder = numberOfItemsPickedForOrder;
            TotalNumberOfItemsInOrder = totalNumberOfItemsInOrder;
            PreviousOrderEventId = previousOrderEventId;
            NextOrderEventId = nextOrderEventId;
        }
    }
}
