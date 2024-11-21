using WMS_API.Models.Items;
using WMS_API.Models.Orders;

namespace WMS_API.Models.Containers
{
    public class Container
    {
        public ContainerDetail ContainerDetail { get; set; }
        public List<Item> Items { get; set; }

        public Container()
        {
        }
        public Container(ContainerDetail containerDetail, List<Item> items)
        {
            ContainerDetail = containerDetail;
            Items = items;
        }
    }
}
