using System;
using WMS_API.Models.Items;

namespace WMS_API.Models.Containers
{
    public class Container
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Item? Item { get; set; }

        public Container(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
