using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Manager
{
    public class RegisterManagerDTO
    {

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string RoleId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public string? PhoneNumber { get; set; }

        public int? SalonId { get; set; }

    }
}
