namespace WMS_API.Models.Boxes
{
    public class BoxData
    {
        public DateTime DateTimeStamp { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public bool IsSealed { get; set; }
        public Guid BoxId { get; set; }
        public Guid? ShipmentId { get; set; }
        public Guid? TruckId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }

        public BoxData()
        {
        }

        public BoxData(
            DateTime eventDateTime,
            string name,
            string description,
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters,
            bool isSealed,
            Guid boxId,
            Guid? shipmentId,
            Guid? truckId,
            Guid? orderId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            Name = name;
            Description = description;
            LengthInCentimeters = lengthInCentimeters;
            WidthInCentimeters = widthInCentimeters;
            HeightInCentimeters = heightInCentimeters;
            IsSealed = isSealed;
            BoxId = boxId;
            ShipmentId = shipmentId;
            TruckId = truckId;
            OrderId = orderId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }

    }
}
