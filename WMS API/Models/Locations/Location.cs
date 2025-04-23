using WMS_API.Models.Items;

namespace WMS_API.Models.Locations
{
    public class Location
    {
        public Guid Id { get; set; }
        public List<LocationData> LocationDataHistory { get; set; }
        public List<ItemData>? LocationItemHistory { get; set; }

        public Location ()
        {
        }

        public Location(
            Guid id,
            List<LocationData> locationDataHistory,
            List<ItemData>? locationItemHistory
        )
        {
            Id = id;
            LocationDataHistory = locationDataHistory;
            LocationItemHistory = locationItemHistory;
        }
    }
}
