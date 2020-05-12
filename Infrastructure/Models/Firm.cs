namespace Infrastructure.Models
{
    public class Firm : BaseEntity
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public int AddressId { get; set; }
    }
}