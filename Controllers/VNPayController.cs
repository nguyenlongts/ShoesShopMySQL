using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Application.DTOs;

namespace ShoesShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VNPayController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] PaymentRequest request)
        {
            string vnp_Url = _configuration["VnPay:PaymentUrl"];
            string vnp_ReturnUrl = _configuration["VnPay:ReturnUrl"];
            string vnp_TmnCode = _configuration["VnPay:TmnCode"];
            string vnp_HashSecret = _configuration["VnPay:HashSecret"];
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vnNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);

            var vnpParams = new Dictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", (request.Amount * 100).ToString() }, 
                { "vnp_CreateDate", vnNow.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1" },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", request.OrderDescription },
                { "vnp_OrderType", request.OrderType },
                { "vnp_ReturnUrl", vnp_ReturnUrl },
                { "vnp_TxnRef", DateTime.Now.Ticks.ToString() } 
            };


            if (!string.IsNullOrEmpty(request.BankCode))
            {
                vnpParams.Add("vnp_BankCode", request.BankCode);
            }

            var sortedParams = new SortedList<string, string>(vnpParams);
            var stringBuilder = new StringBuilder();

            foreach (var param in sortedParams)
            {
                stringBuilder.Append(WebUtility.UrlEncode(param.Key) + "=" + WebUtility.UrlEncode(param.Value) + "&");
            }


            string rawData = stringBuilder.ToString().TrimEnd('&');

            string hashData = HmacSHA512(vnp_HashSecret, rawData);

            string paymentUrl = vnp_Url + "?" + rawData + "&vnp_SecureHash=" + hashData;

            return Ok(new { paymentUrl });
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var result = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            var hex = BitConverter.ToString(result).Replace("-", "").ToLower();
            return hex;
        }
    }
}
