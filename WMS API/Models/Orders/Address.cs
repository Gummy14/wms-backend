namespace WMS_API.Models.Orders
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public Address()
        {
        }

        public Address(
            Guid id,
            Guid orderId,
            string firstName,
            string lastName,
            string street,
            string city,
            string state,
            string zip
        )
        {
            Id = id;
            OrderId = orderId;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            State = state;
            Zip = zip;
        }
    }
}
