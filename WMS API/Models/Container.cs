using System;

namespace WMS_API.Models
{
    public class Container
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ItemId { get; set; }
        public Item? Item { get; set; }
        public Container(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
