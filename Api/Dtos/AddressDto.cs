using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class AddressDto
    {
        
        public string Type { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        

        public string AptAddress { get; set; }
        [Required]

        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]

        public string ZipCode { get; set; }

    }
}