using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Boxes
{
    public class Box : WarehouseObject
    {
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }

        public Box() : base()
        {
        }

        public Box(
            Guid eventId,
            Guid objectId,
            string name,
            string description,
            DateTime eventDateTime,
            int status,
            Guid prevEventId,
            Guid nextEventId,
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
            LengthInCentimeters = lengthInCentimeters;
            WidthInCentimeters = widthInCentimeters;
            HeightInCentimeters = heightInCentimeters;
        }

    }
}
