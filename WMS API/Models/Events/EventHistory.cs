namespace WMS_API.Models.Events
{
    public class EventHistory
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid ChildId { get; set; }
        public int EventType { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public EventHistory(Guid id, Guid parentId, Guid childId, int eventType, DateTime dateTimeStamp)
        {
            Id = id;
            ParentId = parentId;
            ChildId = childId;
            EventType = eventType;
            DateTimeStamp = dateTimeStamp;
        }
    }
}
