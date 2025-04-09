namespace WMS_API.Models.Orders
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public Address()
        {
        }

        public Address(
            Guid id,
            string street,
            string city,
            string state,
            string zip
        )
        {
            Id = id;
            Street = street;
            City = city;
            State = state;
            Zip = zip;
        }
    }
}
