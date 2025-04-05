namespace WMS_API.Models.WarehouseObjects
{
    public class UnregisteredObject
    {
        public Guid? Id { get; set; }
        public int ObjectType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
