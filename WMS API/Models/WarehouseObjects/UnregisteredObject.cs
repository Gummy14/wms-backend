﻿namespace WMS_API.Models.WarehouseObjects
{
    public class UnregisteredObject
    {
        public Guid? Id { get; set; }
        public int ObjectType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
