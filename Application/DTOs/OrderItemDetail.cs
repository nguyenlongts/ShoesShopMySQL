namespace ShoesShop.Application.DTOs
{
    public class OrderItemDetail
    {
        public int Quantity { get; set; }

        public string ColorName { get; set; }

        public string SizeName { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public string ProductName { get; set; }

        public string Image { get; set; }

    }
}
