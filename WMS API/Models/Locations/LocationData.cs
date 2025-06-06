using WMS_API.Models.Items;

namespace WMS_API.Models.Locations
{
    public class LocationData
    {
        public DateTime DateTimeStamp { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int ZCoordinate { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float MaxWeightInKilograms { get; set; }
        public string EventDescription { get; set; }
        public Guid LocationId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }


        public LocationData()
        {
        }

        public LocationData(
            DateTime eventDateTime,
            int xCoordinate,
            int yCoordinate,
            int zCoordinate,
            string description,
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters,
            float maxWeightInKilograms,
            string eventDescription,
            Guid locationId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            ZCoordinate = zCoordinate;
            Description = description;
            LengthInCentimeters = lengthInCentimeters;
            WidthInCentimeters = widthInCentimeters;
            HeightInCentimeters = heightInCentimeters;
            MaxWeightInKilograms = maxWeightInKilograms;
            EventDescription = eventDescription;
            LocationId = locationId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }
    }
}
