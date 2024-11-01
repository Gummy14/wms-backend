using WMS_API.Models.Containers;
using WMS_API.Models.Items;

namespace WMS_API.Models
{
    public class ItemContainer
    {
        public Item Item { get; set; }
        public Container Container { get; set; }
    }
}
