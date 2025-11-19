namespace ShoesShop.Application.DTOs
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; }
        public string OrderType { get; set; } = "other";
        public string BankCode { get; set; } = ""; 
    }
}
