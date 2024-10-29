using WMS_API.Models.Containers;
using WMS_API.Models.Items;

namespace WMS_API.Models.Events
{
    public class ItemContainerEvent
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public Container Container { get; set; }
        public int EventType { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public ItemContainerEvent()
        {
        }
        public ItemContainerEvent(Guid id, Item item, Container container, int eventType, DateTime dateTimeStamp)
        {
            Id = id;
            Item = item;
            Container = container;
            EventType = eventType;
            DateTimeStamp = dateTimeStamp;
        }
    }
}
