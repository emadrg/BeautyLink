using DTOs.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Stylist
{
    public class RegisterStylistDTO
    {
        public int SalonId { get; set; }

        public string? SocialMediaLink { get; set; }

        public List<int> Services { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? StatusId { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public string? PhoneNumber { get; set; }

    }
}
