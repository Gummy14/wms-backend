using WMS_API.Models.Boxes;

namespace WMS_API.Models.Trucks
{
    public class Truck
    {
        public Guid Id { get; set; }
        public Guid ShipmentId { get; set; }
        public List<BoxData>? BoxesLoadedOntoTruck { get; set; }
        public string LicensePlate { get; set; }
        public DateTime ArrivalDateTimeStamp { get; set; }
        public DateTime? DepartureDateTimeStamp { get; set; }

        public Truck()
        {
        }

        public Truck(
            Guid id,
            Guid shipmentId,
            List<BoxData>? boxesLoadedOntoTruck,
            string licensePlate,
            DateTime arrivalDateTimeStamp,
            DateTime? departureDateTimeStamp
        )
        {
            Id = id;
            ShipmentId = shipmentId;
            BoxesLoadedOntoTruck = boxesLoadedOntoTruck;
            LicensePlate = licensePlate;
            ArrivalDateTimeStamp = arrivalDateTimeStamp;
            DepartureDateTimeStamp = departureDateTimeStamp;
        }
    }
}
