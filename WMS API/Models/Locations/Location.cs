using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Locations
{
    public class Location : WarehouseObject
    {
        public Guid ItemId { get; set; }

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
            Guid prevEventId,
            Guid nextEventId,
            Guid itemId
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
            ItemId = itemId;
        }
    }
}
