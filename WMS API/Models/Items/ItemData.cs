namespace WMS_API.Models.Items
{
    public class ItemData
    {
        public DateTime DateTimeStamp { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float WeightInKilograms { get; set; }
        public Guid ItemId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? ContainerId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? BoxId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }
        

        public ItemData()
        {
        }

        public ItemData(
            DateTime eventDateTime,
            string name,
            string description,
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters,
            float weightInKilograms,
            Guid itemId,
            Guid? locationId,
            Guid? containerId,
            Guid? orderId,
            Guid? boxId,
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
            WeightInKilograms = weightInKilograms;
            ItemId = itemId;
            LocationId = locationId;
            ContainerId = containerId;
            OrderId = orderId;
            BoxId = boxId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
    }
}
