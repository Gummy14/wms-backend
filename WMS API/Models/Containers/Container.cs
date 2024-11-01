using System;
using System.ComponentModel;
using WMS_API.Models.Items;

namespace WMS_API.Models.Containers
{
    public class Container
    {
        public Guid ContainerEventId { get; set; }
        public Guid ContainerId { get; set; }
        public string Name { get; set; }
        public Guid ItemEventId { get; set; }
        public DateTime EventDateTime { get; set; }
        public int EventType { get; set; }
        public Guid PreviousContainerEventId { get; set; }
        public Guid NextContainerEventId { get; set; }

        public Container()
        {
        }

        public Container(Guid containerEventId, Guid containerId, string name, Guid itemEventId, DateTime eventDateTime, int eventType, Guid prevEventId, Guid nextEventId)
        {
            ContainerEventId = containerEventId;
            ContainerId = containerId;
            Name = name;
            ItemEventId = itemEventId;
            EventDateTime = eventDateTime;
            EventType = eventType;
            PreviousContainerEventId = prevEventId;
            NextContainerEventId = nextEventId;
        }
    }
}
