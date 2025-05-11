using WMS_API.Models.Items;

namespace WMS_API.Models.Orders
{
    public class OrderData
    {
        public DateTime DateTimeStamp { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventDescription { get; set; }
        public Guid OrderId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }

        public OrderData()
        {
        }

        public OrderData(
            DateTime eventDateTime,
            string name,
            string description,
            string eventDescription,
            Guid orderId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            Name = name;
            Description = description;
            EventDescription = eventDescription;
            OrderId = orderId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
    }
}
