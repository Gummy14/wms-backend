using WMS_API.Models.Boxes;
using WMS_API.Models.Trucks;

namespace WMS_API.Models.Shipments
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public List<ShipmentData> ShipmentData { get; set; }
        public List<BoxData>? ShipmentBoxes { get; set; }
        public List<Truck>? TruckData { get; set; }

        public Shipment()
        {
        }

        public Shipment(
            Guid id,
            List<ShipmentData> shipmentData,
            List<BoxData>? shipmentBoxes,
            List<Truck>? truckData
        )
        {
            Id = id;
            ShipmentData = shipmentData;
            ShipmentBoxes = shipmentBoxes;
            TruckData = truckData;
        }
    }
}
