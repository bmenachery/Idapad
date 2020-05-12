using System;

namespace Infrastructure.Models
{
    public class FirmProduct: BaseEntity
    {
        public string FirmName { get; set; }

        public string FirmType { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public string PictureUrl { get; set; }

        public string ProductType { get; set; }

        public string ProductBrand { get; set; }

        public int ProductId { get; set; }

        public DateTime? DateCreated { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductBrandId { get; set; }

        public int FirmId { get; set; }


    }
}