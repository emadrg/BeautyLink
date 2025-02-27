using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Stylist
{
    public class StylistDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int SalonId { get; set; }

        public string Salon { get; set; }

        public string? SocialMediaLink { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public int? ProfilePictureId { get; set; }

        public string? ProfilePicture { get; set; }

        public string? PhoneNumber { get; set; }

        public double? AverageScore { get; set; }

    }
}
