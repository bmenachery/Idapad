namespace Api.Dtos
{
    public class OrderItemDto
    {
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }

       
    }
}