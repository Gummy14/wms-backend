using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Items
{
    public class Item : WarehouseObject
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public Guid ContainerId { get; set; }
        public string ContainerName { get; set; }
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }

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
            string locationName,
            Guid containerId,
            string containerName,
            Guid orderId,
            string orderName
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
            LocationId = locationId;
            LocationName = locationName;
            ContainerId = containerId;
            ContainerName = containerName;
            OrderId = orderId;
            OrderName = orderName;
        }
    }
}
