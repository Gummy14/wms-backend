using WMS_API.Models.Items;

namespace WMS_API.Models.Locations
{
    public class Location
    {
        public Guid Id { get; set; }
        public List<LocationData> LocationData { get; set; }
        public List<ItemData>? LocationItem { get; set; }
        public List<Location>? SubLocations { get; set; }
        public Guid? LocationParentId { get; set; }
        public Location ParentLocation { get; set; }

        public Location ()
        {
        }

        public Location(
            Guid id,
            List<LocationData> locationData,
            List<ItemData>? locationItem,
            List<Location>? subLocations,
            Guid? locationParentId,
            Location parentLocation

        )
        {
            Id = id;
            LocationData = locationData;
            LocationItem = locationItem;
            SubLocations = subLocations;
            LocationParentId = locationParentId;
            ParentLocation = parentLocation;
        }
    }
}
