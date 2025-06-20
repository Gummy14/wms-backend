﻿namespace WMS_API.Models.WarehouseObjects
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

        public UnregisteredObject()
        {
        }

        public UnregisteredObject
        (
            Guid? id, 
            int objectType, 
            string name, 
            string description, 
            float lengthInCentimeters, 
            float widthInCentimeters, 
            float heightInCentimeters, 
            float weightOrMaxWeightInKilograms
        )
        {
            Id = id;
            ObjectType = objectType;
            Name = name;
            Description = description;
            LengthInCentimeters = lengthInCentimeters;
            WidthInCentimeters = widthInCentimeters;
            HeightInCentimeters = heightInCentimeters;
            WeightOrMaxWeightInKilograms = weightOrMaxWeightInKilograms;
        }
    }
}
