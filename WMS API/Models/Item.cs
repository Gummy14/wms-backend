using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS_API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }

        public Item(string name, string description, int statusId)
        {
            Id = 0;
            Name = name;
            Description = description;
            StatusId = statusId;
        }
    }
}
