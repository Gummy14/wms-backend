namespace WMS_API.Models.WarehouseObjects
{
    public class WarehouseObjectWithChildren
    {
        public WarehouseObject WarehouseParentObject { get; set; }
        public List<WarehouseObject> WarehouseChildrenObjects { get; set; }

        public WarehouseObjectWithChildren()
        {
        }
        public WarehouseObjectWithChildren(WarehouseObject warehouseParentObject, List<WarehouseObject> warehouseChildrenObjects)
        {
            WarehouseParentObject = warehouseParentObject;
            WarehouseChildrenObjects = warehouseChildrenObjects;
        }
    }
}
