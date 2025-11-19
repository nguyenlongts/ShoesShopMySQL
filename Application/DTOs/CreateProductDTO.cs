namespace ShoesShop.Application.DTOs
{
    public class CreateProductDTO
    {
        public  string ProductName { get; set; }

        public string Description { get; set; }

        public float BasePrice { get; set; }
        public int BrandID { get; set; }

        public int CateID { get; set; }
    }
}
