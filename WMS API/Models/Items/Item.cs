using WMS_API.Models.Locations;

namespace WMS_API.Models.Items
{
    public class Item
    {
        public Guid Id { get; set; }
        public List<ItemData> ItemDataHistory { get; set; }
        public List<LocationData>? Location { get; set; }

        public Item ()
        {
        }

        public Item(
            Guid id,
            List<ItemData> itemDataHistory,
            List<LocationData>? location
        )
        {
            Id = id;
            ItemDataHistory = itemDataHistory;
            Location = location;
        }
    }
}
