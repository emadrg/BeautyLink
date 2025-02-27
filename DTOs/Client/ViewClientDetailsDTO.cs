using DTOs.Appointment;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Client
{
    public class ViewClientDetailsDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string? ProfilePicture { get; set; }

        public string? PhoneNumber { get; set; }

        public List<AppointmentDTO> Appointments { get; set; } = null!;
    }
}
