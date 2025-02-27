using DTOs.ServiceStylist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Appointment
{
    public class AppointmentDTO
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; } = null!;

        public Guid ClientId { get; set; }

        public string ClientName { get; set; } = null!;

        public List <ServiceStylistDTO> Services { get; set; } = new List<ServiceStylistDTO>();
    }
}
