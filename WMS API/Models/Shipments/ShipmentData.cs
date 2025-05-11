namespace WMS_API.Models.Shipments
{
    public class ShipmentData
    {
        public DateTime DateTimeStamp { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventDescription { get; set; }
        public Guid ShipmentId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }

        public ShipmentData()
        {
        }

        public ShipmentData(
            DateTime eventDateTime,
            string name,
            string description,
            string eventDescription,
            Guid shipmentId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            Name = name;
            Description = description;
            EventDescription = eventDescription;
            ShipmentId = shipmentId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
    }
}
