﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ServiceStylist
{
    public class CreateAppointmentServiceStylistDTO
    {
        public int ServiceId { get; set; }

        public Guid StylistId { get; set; }
    }
}
