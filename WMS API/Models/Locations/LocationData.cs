using WMS_API.Models.Items;

namespace WMS_API.Models.Locations
{
    public class LocationData
    {
        public DateTime DateTimeStamp { get; set; }
        public int Zone { get; set; }
        public int Shelf {  get; set; }
        public int Row {  get; set; }
        public int Column { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float MaxWeightInKilograms { get; set; }
        public string EventDescription { get; set; }
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
            int zone,
            int shelf,
            int row,
            int column,
            string description,
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters,
            float maxWeightInKilograms,
            string eventDescription,
            Guid locationId,
            Guid? itemId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            Zone = zone;
            Shelf = shelf;
            Row = row;
            Column = column;
            Description = description;
            LengthInCentimeters = lengthInCentimeters;
            WidthInCentimeters = widthInCentimeters;
            HeightInCentimeters = heightInCentimeters;
            MaxWeightInKilograms = maxWeightInKilograms;
            EventDescription = eventDescription;
            LocationId = locationId;
            ItemId = itemId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
    }
}
