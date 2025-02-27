using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Reviews
{
    public class UpdateClientStylistReviewDTO
    {
        public Guid StylistId { get; set; }

        public string? Text { get; set; }

        public int Score { get; set; }
    }
}
