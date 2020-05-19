namespace Infrastructure.Models
{
    public class FirmAddress
    {
        public string Type { get; set; }

        public string StreetAddress { get; set; }
        public string AptAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}