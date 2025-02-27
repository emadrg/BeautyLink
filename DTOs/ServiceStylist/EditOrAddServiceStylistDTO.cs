using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ServiceStylist
{
    public class EditOrAddServiceStylistDTO
    {
        public int ServiceId { get; set; }

        public Guid StylistId { get; set; }

        public int DurationMinutes { get; set; }

        public decimal Price { get; set; }
    }
}
