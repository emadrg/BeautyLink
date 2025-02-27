using DTOs.ServiceStylist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Appointment
{
    public class CreateAppointmentDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<CreateAppointmentServiceStylistDTO> Services { get; set; } = new List<CreateAppointmentServiceStylistDTO>();
    }
}
