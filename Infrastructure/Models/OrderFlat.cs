using System;

namespace Infrastructure.Models
{
    public class OrderFlat
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }

        public string BasketId { get; set; }
        public DateTime OrderDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string AptAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string ZipCode { get; set; }

        public string DelShortName { get; set; }

        public string DeliveryTime { get; set; }

        public string DelDescription { get; set; }

        public decimal DelPrice { get; set; }

        public int FirmProductId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductPictureUrl { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Subtotal { get; set; }

        public int SizeId { get; set; }

        public string Status { get; set; }

        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        
    }
}