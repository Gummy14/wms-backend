namespace WMS_API.Models.WarehouseObjects
{
    public class WarehouseParentObjectWithChildren
    {
        public WarehouseObject WarehouseParentObject { get; set; }
        public List<WarehouseObject> WarehouseChildrenObjects { get; set; }

        public WarehouseParentObjectWithChildren()
        {
        }
        public WarehouseParentObjectWithChildren(WarehouseObject warehouseParentObject, List<WarehouseObject> warehouseChildrenObjects)
        {
            WarehouseParentObject = warehouseParentObject;
            WarehouseChildrenObjects = warehouseChildrenObjects;
        }
    }
}
