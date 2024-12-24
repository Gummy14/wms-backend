using WMS_API.Models.Items;
using WMS_API.Models.WarehouseObjects;

namespace WMS_API.Models.Orders
{
    public class Order : WarehouseObject
    {
        public List<Item> OrderItems { get; set; }

        public Order() : base()
        {
        }

        public Order(
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

        public Order(
            Guid eventId,
            Guid objectId,
            string name,
            string description,
            DateTime eventDateTime,
            int status,
            Guid prevEventId,
            Guid nextEventId,
            List<Item> orderItems
        ) : base(eventId, objectId, name, description, eventDateTime, status, prevEventId, nextEventId)
        {
            OrderItems = orderItems;
        }
    }
}
