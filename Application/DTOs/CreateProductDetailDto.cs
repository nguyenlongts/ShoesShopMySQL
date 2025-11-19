namespace ShoesShop.Application.DTOs
{
    public class CreateProductDetailDTO
    {
        public int ProductId { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
    }
}
