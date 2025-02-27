using DTOs.Service;
using Microsoft.AspNetCore.Http;

namespace DTOs.Users
{
    public class RegisterUserDTO
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string RoleId { get; set; } = null!;
        
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public string? PhoneNumber { get; set; }

        public int? SalonId { get; set; }

        public List<int>? ServiceIds { get; set; }

    }
}
