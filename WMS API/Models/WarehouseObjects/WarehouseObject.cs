namespace WMS_API.Models.WarehouseObjects
{
    public class WarehouseObject
    {
        public Guid EventId { get; set; }
        public int ObjectType { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public Guid ContainerId { get; set; }
        public string ContainerName { get; set; }
        public string ContainerDescription { get; set; }
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
        public string OrderDescription { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public int Status { get; set; }
        public Guid PreviousEventId { get; set; }
        public Guid NextEventId { get; set; }
        

        public WarehouseObject()
        {
        }

        public WarehouseObject(
            Guid eventId,
            int objectType,
            Guid itemId,
            string itemName,
            string itemDescription,
            Guid locationId,
            string locationName,
            string locationDescription,
            Guid containerId,
            string containerName,
            string containerDescription,
            Guid orderId,
            string orderName,
            string orderDescription,
            DateTime dateTimeStamp,
            int status,
            Guid previousEventId,
            Guid nextEventId
            )
        {
            EventId = eventId;
            ObjectType = objectType;
            ItemId = itemId;
            ItemName = itemName;
            ItemDescription = itemDescription;
            LocationId = locationId;
            LocationName = locationName;
            LocationDescription = locationDescription;
            ContainerId = containerId;
            ContainerName = containerName;
            ContainerDescription = containerDescription;
            OrderId = orderId;
            OrderName = orderName;
            OrderDescription = orderDescription;
            DateTimeStamp = dateTimeStamp;
            Status = status;
            PreviousEventId = previousEventId;
            NextEventId = nextEventId;
        }
    }
}
