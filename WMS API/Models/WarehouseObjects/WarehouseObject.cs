namespace WMS_API.Models.WarehouseObjects
{
    public class WarehouseObject
    {
        public Guid EventId { get; set; }
        public Guid ObjectId { get; set; }
        public int ObjectType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDateTime { get; set; }
        public int EventType { get; set; }
        public Guid PreviousEventId { get; set; }
        public Guid NextEventId { get; set; }

        public WarehouseObject()
        {
        }

        public WarehouseObject(Guid eventId, Guid objectId, int objectType, string name, string description, DateTime eventDateTime, int eventType, Guid prevEventId, Guid nextEventId)
        {
            EventId = eventId;
            ObjectId = objectId;
            ObjectType = objectType;
            Name = name;
            Description = description;
            EventDateTime = eventDateTime;
            EventType = eventType;
            PreviousEventId = prevEventId;
            NextEventId = nextEventId;
        }
    }
}
