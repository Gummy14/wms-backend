namespace WMS_API.Models
{
    public class Container
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Container(int id, int itemId)
        {
            Id = id;
            this.ItemId = itemId;
        }
    }
}
