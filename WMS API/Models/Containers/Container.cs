using WMS_API.Models.Items;

namespace WMS_API.Models.Containers
{
    public class Container
    {
        public Guid Id { get; set; }
        public List<ContainerData> ContainerData { get; set; }
        public List<ItemData>? ContainerItems { get; set; }

        public Container()
        {
        }

        public Container(
            Guid id,
            List<ContainerData> containerData,
            List<ItemData>? containerItems
        )
        {
            Id = id;
            ContainerData = containerData;
            ContainerItems = containerItems;
        }
    }
}
