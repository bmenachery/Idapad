namespace Infrastructure.Models
{
    public class Address: BaseEntity
    {
        public string Type { get; set; }

        public string Row1 { get; set; }
        public string Row2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip1 { get; set; }
    }
}