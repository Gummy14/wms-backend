using WMS_API.Models.Containers;
using WMS_API.Models.Items;

namespace WMS_API.Models.Boxes
{
    public class Box
    {
        public Guid Id { get; set; }
        public List<BoxData> BoxData { get; set; }
        public List<ItemData>? BoxItems { get; set; }

        public Box()
        {
        }

        public Box(
            Guid id,
            List<BoxData> boxData,
            List<ItemData>? boxItems
        )
        {
            Id = id;
            BoxData = boxData;
            BoxItems = boxItems;
        }
    }
}
