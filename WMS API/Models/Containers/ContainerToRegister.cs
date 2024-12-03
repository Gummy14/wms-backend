namespace WMS_API.Models.Containers
{
    public class ContainerToRegister
    {
        public Guid? ContainerId { get; set; }
        public string Name { get; set; }
        public int ContainerType { get; set; }
    }
}
