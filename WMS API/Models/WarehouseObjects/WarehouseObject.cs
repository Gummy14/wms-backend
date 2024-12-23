namespace WMS_API.Models.WarehouseObjects
{
    public class WarehouseObject
    {
        public Guid EventId { get; set; }
        public Guid ObjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDateTime { get; set; }
        public int Status { get; set; }
        public Guid PreviousEventId { get; set; }
        public Guid NextEventId { get; set; }

        public WarehouseObject()
        {
        }

        public WarehouseObject(Guid eventId, Guid objectId, string name, string description, DateTime eventDateTime, int status, Guid prevEventId, Guid nextEventId)
        {
            EventId = eventId;
            ObjectId = objectId;
            Name = name;
            Description = description;
            EventDateTime = eventDateTime;
            Status = status;
            PreviousEventId = prevEventId;
            NextEventId = nextEventId;
        }
    }
}
