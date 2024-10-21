namespace WMS_API.Models
{
    public class Container
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public Item? Item { get; set; }
        public Container(int id)
        {
            Id = id;
        }
    }
}
