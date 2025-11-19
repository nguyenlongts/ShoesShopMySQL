namespace API_ShoesShop.Application.DTOs
{
    public class RegisterDTO
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateOnly DoB { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}
