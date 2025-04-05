namespace WMS_API.Models.WarehouseObjects
{
    public class UnregisteredObject
    {
        public Guid? Id { get; set; }
        public int ObjectType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public float WeightOrMaxWeightInKilograms { get; set; }
    }
}
