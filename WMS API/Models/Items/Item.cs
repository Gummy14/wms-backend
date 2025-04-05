using Microsoft.AspNetCore.Routing.Constraints;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Items
{
    public class Item : WarehouseObject
    {
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float WeightInKilograms { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public Guid ContainerId { get; set; }
        public string ContainerName { get; set; }
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
        public Guid BoxId { get; set; }
        public string BoxName { get; set; }
        
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
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters,
            float weightInKilograms,
            Guid prevEventId, 
            Guid nextEventId,
            Guid locationId,
            string locationName,
            Guid containerId,
            string containerName,
            Guid orderId,
            string orderName,
            Guid boxId,
            string boxName
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
            LengthInCentimeters = lengthInCentimeters;
            WidthInCentimeters = widthInCentimeters;
            HeightInCentimeters = heightInCentimeters;
            WeightInKilograms = weightInKilograms;
            LocationId = locationId;
            LocationName = locationName;
            ContainerId = containerId;
            ContainerName = containerName;
            OrderId = orderId;
            OrderName = orderName;
            BoxId = boxId;
            BoxName = boxName;
        }
    }
}
