namespace WMS_API.Models.Locations
{
    public class UnregisteredLocation
    {
        public Guid? Id { get; set; }
        public int ObjectType { get; set; }
        public int Zone { get; set; }
        public int Shelf { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float WeightOrMaxWeightInKilograms { get; set; }
    }
}
