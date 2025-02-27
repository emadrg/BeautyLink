using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ServiceStylist
{
    public class ServiceStylistDTO
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }

        public string Service { get; set; }

        public Guid StylistId { get; set; }

        public string Stylist { get; set; }

        public int DurationMinutes { get; set; }

        public decimal Price { get; set; }
    }
}
