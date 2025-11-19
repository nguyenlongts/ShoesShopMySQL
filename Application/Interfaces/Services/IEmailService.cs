namespace ShoesShop.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendMailAsync(string address, string subject, string body);
    }
}
