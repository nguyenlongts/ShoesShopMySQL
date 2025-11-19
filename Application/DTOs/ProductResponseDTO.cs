namespace ShoesShop.Application.DTOs
{
    public class ProductResponseDTO
    {
        public int TotalProducts { get; set; }
        public List<GetProductDTO> Products { get; set; }

        public int TotalPages { get; set; }
    }

}
