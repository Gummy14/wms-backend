using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS_API.Models.Containers;
using WMS_API.Models.Orders;

namespace WMS_API.Models.Items
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeRegistered { get; set; }

        public Item(Guid id, string name, string description, DateTime dateTimeRegistered)
        {
            Id = id;
            Name = name;
            Description = description;
            DateTimeRegistered = dateTimeRegistered;
        }
    }
}
