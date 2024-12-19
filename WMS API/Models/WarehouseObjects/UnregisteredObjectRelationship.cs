namespace WMS_API.Models.WarehouseObjects
{
    public class UnregisteredObjectRelationship
    {
        public WarehouseObject Parent { get; set; }
        public WarehouseObject Child { get; set; }
    }
}
