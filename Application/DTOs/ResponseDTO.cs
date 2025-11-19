namespace ShoesShop.Application.DTOs
{
        public class ResponseDTO<T>
        {
            public List<T> Items { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages { get; set; }
        }
}
