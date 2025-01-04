using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Containers
{
    public class Container : WarehouseObject
    {
        public Container() : base()
        {
        }

        public Container(
            Guid eventId,
            Guid objectId,
            string name,
            string description,
            DateTime eventDateTime,
            int status,
            Guid prevEventId,
            Guid nextEventId
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
        }
        
    }
}
