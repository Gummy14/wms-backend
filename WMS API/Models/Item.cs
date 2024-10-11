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
        public string Status { get; set; }
        public string PutawayLocation { get; set; }

        public Item(string name, string description, string status, string putawayLocation)
        {
            Id = 0;
            Name = name;
            Description = description;
            Status = status;
            PutawayLocation = putawayLocation;
        }
    }
}
