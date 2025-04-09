namespace WMS_API.Models.Boxes
{
    public class BoxData
    {
        public DateTime DateTimeStamp { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float LengthInCentimeters { get; set; }
        public float WidthInCentimeters { get; set; }
        public float HeightInCentimeters { get; set; }
        public Guid BoxId { get; set; }
        public Guid EventId { get; set; }
        public Guid? NextEventId { get; set; }
        public Guid? PrevEventId { get; set; }

        public BoxData()
        {
        }

        public BoxData(
            DateTime eventDateTime,
            int status,
            string name,
            string description,
            float lengthInCentimeters,
            float widthInCentimeters,
            float heightInCentimeters,
            Guid boxId,
            Guid eventId,
            Guid? nextEventId,
            Guid? prevEventId
        )
        {
            DateTimeStamp = eventDateTime;
            Status = status;
            Name = name;
            Description = description;
            LengthInCentimeters = lengthInCentimeters;
            WidthInCentimeters = widthInCentimeters;
            HeightInCentimeters = heightInCentimeters;
            BoxId = boxId;
            EventId = eventId;
            NextEventId = nextEventId;
            PrevEventId = prevEventId;
        }

    }
}
