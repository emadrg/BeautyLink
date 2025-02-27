using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Reviews
{
    public class UpdateClientSalonReviewDTO
    {
        public int SalonId { get; set; }

        public string? Text { get; set; }

        public int Score { get; set; }
    }
}
