namespace Infrastructure.Models
{
    public class OrderAddress
    {
        public OrderAddress(string firstName, string lastName, string streetAddress, string aptAddress, string city, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            StreetAddress = streetAddress;
            AptAddress = aptAddress;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string AptAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string ZipCode { get; set; }
    }
}