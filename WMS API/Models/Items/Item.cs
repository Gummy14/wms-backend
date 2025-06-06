using WMS_API.Models.Locations;

namespace WMS_API.Models.Items
{
    public class Item
    {
        public Guid Id { get; set; }
        public List<ItemData> ItemData { get; set; }

        public Item ()
        {
        }

        public Item(
            Guid id,
            List<ItemData> itemData
        )
        {
            Id = id;
            ItemData = itemData;
        }
    }
}
