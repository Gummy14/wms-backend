namespace WMS_API.Models.WarehouseObjects
{
    public class WarehouseObjectRelationship
    {
        public Guid EventId { get; set; }
        public Guid RelationshipId { get; set; }
        public Guid ParentId { get; set; }
        public Guid ChildId { get; set; }
        public DateTime EventDateTime { get; set; }
        //public int EventType { get; set; }
        public Guid PreviousEventId { get; set; }
        public Guid NextEventId { get; set; }

        public WarehouseObjectRelationship()
        {
        }

        public WarehouseObjectRelationship(Guid eventId, Guid relationshipId, Guid parentId, Guid childId, DateTime eventDateTime, Guid prevEventId, Guid nextEventId)
        {
            EventId = eventId;
            RelationshipId = relationshipId;
            ParentId = parentId;
            ChildId = childId;
            EventDateTime = eventDateTime;
            //EventType = eventType;
            PreviousEventId = prevEventId;
            NextEventId = nextEventId;
        }
    }
}
