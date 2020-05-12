namespace Api.Dtos
{
    public class FirmProductToReturnDto
    {
        public int Id { get; set; }
        public string FirmName { get; set; }
        public string FirmType { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
    }
}