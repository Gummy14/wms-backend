namespace WMS_API.Models
{
    public class ItemContainerEvent
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid ContainerId { get; set; }
        public int EventType { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public ItemContainerEvent(Guid id, Guid itemId, Guid containerId, int eventType, DateTime dateTimeStamp)
        {
            Id = id;
            ItemId = itemId;
            ContainerId = containerId;
            EventType = eventType;
            DateTimeStamp = dateTimeStamp;
        }
    }
}
