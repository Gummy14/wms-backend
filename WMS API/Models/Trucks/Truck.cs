using WMS_API.Models.Shipments;

namespace WMS_API.Models.Trucks
{
    public class Truck
    {
        public Guid Id { get; set; }
        public List<ShipmentData>? TruckShipment { get; set; }
        public string LicensePlate { get; set; }
        public DateTime ArrivalDateTimeStamp { get; set; }
        public DateTime? DepartureDateTimeStamp { get; set; }

        public Truck()
        {
        }

        public Truck(
            Guid id,
            Guid shipmentId,
            List<ShipmentData>? truckShipment,
            string licensePlate,
            DateTime arrivalDateTimeStamp,
            DateTime? departureDateTimeStamp
        )
        {
            Id = id;
            TruckShipment = truckShipment;
            LicensePlate = licensePlate;
            ArrivalDateTimeStamp = arrivalDateTimeStamp;
            DepartureDateTimeStamp = departureDateTimeStamp;
        }
    }
}
