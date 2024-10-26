using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS_API.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Item(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
