namespace WMS_API.Models.Locations
{
    public class UnregisteredLocation
    {
        public Guid? Id { get; set; }
        public int ObjectType { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int ZCoordinate { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float WeightOrMaxWeightInKilograms { get; set; }
    }
}
