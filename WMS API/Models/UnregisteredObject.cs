namespace WMS_API.Models
{
    public class UnregisteredObject
    {
        public Guid? ObjectId { get; set; }
        public int ObjectType { get; set; }
        public UnregisteredObjectData ObjectData { get; set; }
    }
}
