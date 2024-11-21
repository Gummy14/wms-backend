using System;
using System.ComponentModel;
using WMS_API.Models.Items;

namespace WMS_API.Models.Containers
{
    public class ContainerDetail
    {
        public Guid ContainerEventId { get; set; }
        public Guid ContainerId { get; set; }
        public string Name { get; set; }
        public bool IsFull { get; set; }
        public int ContainerType { get; set; }
        public DateTime EventDateTime { get; set; }
        public int EventType { get; set; }
        public Guid PreviousContainerEventId { get; set; }
        public Guid NextContainerEventId { get; set; }

        public ContainerDetail()
        {
        }

        public ContainerDetail(Guid containerEventId, Guid containerId, string name, bool isFull, int containerType, DateTime eventDateTime, int eventType, Guid prevEventId, Guid nextEventId)
        {
            ContainerEventId = containerEventId;
            ContainerId = containerId;
            Name = name;
            IsFull = isFull;
            ContainerType = containerType;
            EventDateTime = eventDateTime;
            EventType = eventType;
            PreviousContainerEventId = prevEventId;
            NextContainerEventId = nextEventId;
        }
    }
}
