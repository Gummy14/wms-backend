using WMS_API.Models.Containers;
using WMS_API.Models.Items;

namespace WMS_API.Models.Boxes
{
    public class Box
    {
        public Guid Id { get; set; }
        public List<BoxData> BoxDataHistory { get; set; }
        public List<ItemData>? BoxItems { get; set; }

        public Box()
        {
        }

        public Box(
            Guid id,
            List<BoxData> boxDataHistory,
            List<ItemData>? boxItems
        )
        {
            Id = id;
            BoxDataHistory = boxDataHistory;
            BoxItems = boxItems;
        }
    }
}
