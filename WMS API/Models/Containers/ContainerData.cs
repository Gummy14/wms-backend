namespace WMS_API.Models.Containers
{
    public class ContainerData
    {
        public DateTime DateTimeStamp { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ContainerId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }

        public ContainerData()
        {
        }

        public ContainerData(
            DateTime eventDateTime,
            int status,
            string name,
            string description,
            Guid containerId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            Status = status;
            Name = name;
            Description = description;
            ContainerId = containerId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
        
    }
}
