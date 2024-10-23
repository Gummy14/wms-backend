namespace WMS_API.Models
{
    public class ItemContainerEvent
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ContainerId { get; set; }
        public int EventType { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public ItemContainerEvent(int itemId, int containerId, int eventType, DateTime dateTimeStamp)
        {
            ItemId = itemId;
            ContainerId = containerId;
            EventType = eventType;
            DateTimeStamp = dateTimeStamp;
        }
    }
}
