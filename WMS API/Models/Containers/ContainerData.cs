namespace WMS_API.Models.Containers
{
    public class ContainerData
    {
        public DateTime DateTimeStamp { get; set; }
        public int EventType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ContainerId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }

        public ContainerData()
        {
        }

        public ContainerData(
            DateTime eventDateTime,
            int eventType,
            string name,
            string description,
            Guid containerId,
            Guid? orderId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            EventType = eventType;
            Name = name;
            Description = description;
            ContainerId = containerId;
            OrderId = orderId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
        
    }
}
