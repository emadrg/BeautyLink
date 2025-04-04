﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.StylistUnavailableTime
{
    public class CreateUnavailableTimeDTO
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Reason { get; set; }

        public Guid StylistId { get; set; }
    }
}
