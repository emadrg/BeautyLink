using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Schedule
{
    public class UpdateScheduleDTO
    {
        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
