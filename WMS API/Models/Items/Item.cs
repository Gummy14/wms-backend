using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Items
{
    public class Item : WarehouseObject
    {
        public Guid LocationId { get; set; }
        public Guid ContainerId { get; set; }
        public Guid OrderId { get; set; }

        public Item() : base()
        {
        }

        public Item(
            Guid eventId, 
            Guid objectId, 
            string name, 
            string description, 
            DateTime eventDateTime, 
            int status, 
            Guid prevEventId, 
            Guid nextEventId,
            Guid locationId,
            Guid containerId,
            Guid orderId
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
            LocationId = locationId;
            ContainerId = containerId;
            OrderId = orderId;
        }
    }
}
