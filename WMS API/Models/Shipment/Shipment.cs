using WMS_API.Models.Boxes;

namespace WMS_API.Models.Shipment
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public List<ShipmentData> ShipmentDataHistory { get; set; }
        public List<BoxData>? ShipmentBoxes { get; set; }

        public Shipment()
        {
        }

        public Shipment(
            Guid id,
            List<ShipmentData> shipmentDataHistory,
            List<BoxData>? shipmentBoxes
        )
        {
            Id = id;
            ShipmentDataHistory = shipmentDataHistory;
            ShipmentBoxes = shipmentBoxes;
        }
    }
}
