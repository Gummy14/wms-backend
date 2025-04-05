using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Boxes
{
    public class Box : WarehouseObject
    {
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

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
            float length,
            float width,
            float height
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
            Length = length;
            Width = width;
            Height = height;
        }

    }
}
