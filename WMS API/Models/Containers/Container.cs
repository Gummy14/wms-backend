using WMS_API.Models.Items;

namespace WMS_API.Models.Containers
{
    public class Container
    {
        public Guid Id { get; set; }
        public List<ContainerData> ContainerDataHistory { get; set; }
        public List<ItemData>? ContainerItems { get; set; }

        public Container()
        {
        }

        public Container(
            Guid id,
            List<ContainerData> containerDataHistory,
            List<ItemData>? containerItems
        )
        {
            Id = id;
            ContainerDataHistory = containerDataHistory;
            ContainerItems = containerItems;
        }
    }
}
