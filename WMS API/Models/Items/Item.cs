using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS_API.Models.Containers;
using WMS_API.Models.Events;
using WMS_API.Models.Orders;

namespace WMS_API.Models.Items
{
    public class Item
    {
        public Guid ItemEventId { get; set; }
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? OrderId { get; set; }
        public DateTime EventDateTime { get; set; }
        public int EventType { get; set; }
        public Guid PreviousItemEventId { get; set; }
        public Guid NextItemEventId { get; set; }

        public Item()
        {
        }

        public Item(Guid itemEventId, Guid itemId, string name, string description, Guid? orderId, DateTime eventDateTime, int eventType, Guid prevEventId, Guid nextEventId)
        {
            ItemEventId = itemEventId;
            ItemId = itemId;
            Name = name;
            Description = description;
            OrderId = orderId;
            EventDateTime = eventDateTime;
            EventType = eventType;
            PreviousItemEventId = prevEventId;
            NextItemEventId = nextEventId;
        }
    }
}
