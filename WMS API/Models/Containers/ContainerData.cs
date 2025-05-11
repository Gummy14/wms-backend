namespace WMS_API.Models.Containers
{
    public class ContainerData
    {
        public DateTime DateTimeStamp { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventDescription { get; set; }
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
            string name,
            string description,
            string eventDescription,
            Guid containerId,
            Guid? orderId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            Name = name;
            Description = description;
            EventDescription = eventDescription;
            ContainerId = containerId;
            OrderId = orderId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
        
    }
}
