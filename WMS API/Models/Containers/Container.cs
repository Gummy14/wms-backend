using System;
using WMS_API.Models.Items;

namespace WMS_API.Models.Containers
{
    public class Container
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Item? Item { get; set; }
        public DateTime DateTimeRegistered { get; set; }

        public Container(Guid id, string name, DateTime dateTimeRegistered)
        {
            Id = id;
            Name = name;
            DateTimeRegistered = dateTimeRegistered;
        }
    }
}
