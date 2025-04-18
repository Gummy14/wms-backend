using WMS_API.Models.Items;

namespace WMS_API.Models.Locations
{
    public class LocationData
    {
        public DateTime DateTimeStamp { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float MaxWeightInKilograms { get; set; }
        public Guid LocationId { get; set; }
        public Guid? ItemId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }

        public LocationData()
        {
        }

        public LocationData(
            DateTime eventDateTime,
            string name,
            string description,
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters,
            float maxWeightInKilograms,
            Guid locationId,
            Guid? itemId,
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
            MaxWeightInKilograms = maxWeightInKilograms;
            LocationId = locationId;
            ItemId = itemId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
    }
}
