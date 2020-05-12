using System;

namespace Infrastructure.Models
{
    public class Product : BaseEntity
    {

        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductBrandId { get; set; }

        public DateTime? DateCreated { get; set; }

        public string PictureUrl { get; set; }

        public decimal? Price { get; set; }

        public string Description { get; set; }

        public string ProductBrand { get; set; }

        public string ProductType { get; set; }
    }
}