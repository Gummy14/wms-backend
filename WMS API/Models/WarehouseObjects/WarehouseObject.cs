namespace WMS_API.Models.WarehouseObjects
{
    public class WarehouseObject
    {
        public Guid EventId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public int Status { get; set; }
        public Guid PreviousEventId { get; set; }
        public Guid NextEventId { get; set; }

        public WarehouseObject()
        {
        }

        public WarehouseObject(Guid eventId, Guid objectId, string name, string description, DateTime dateTime, int status, Guid prevEventId, Guid nextEventId)
        {
            EventId = eventId;
            Id = objectId;
            Name = name;
            Description = description;
            DateTimeStamp = dateTime;
            Status = status;
            PreviousEventId = prevEventId;
            NextEventId = nextEventId;
        }
    }
}
