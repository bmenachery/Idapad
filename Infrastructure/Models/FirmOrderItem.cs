namespace Infrastructure.Models
{
    public class FirmOrderItem: BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public FirmProductItem ItemOrdered { get; set; }
   
    }
}