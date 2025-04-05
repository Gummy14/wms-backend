using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Locations
{
    public class Location : WarehouseObject
    {
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float MaxWeightInKilograms { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }

        public Location() : base()
        {
        }

        public Location(
            Guid eventId,
            Guid objectId,
            string name,
            string description,
            DateTime eventDateTime,
            int status,
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters,
            float maxWeightInKilograms,
            Guid prevEventId,
            Guid nextEventId,
            Guid itemId,
            string itemName
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
            LengthInCentimeters = lengthInCentimeters;
            WidthInCentimeters = widthInCentimeters;
            HeightInCentimeters = heightInCentimeters;
            MaxWeightInKilograms = maxWeightInKilograms;
            ItemId = itemId;
            ItemName = itemName;
        }
    }
}
