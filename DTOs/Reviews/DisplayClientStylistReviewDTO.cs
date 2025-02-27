using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Reviews
{
    public class DisplayClientStylistReviewDTO
    {
        public int Id { get; set; }

        public string ClientName { get; set; }

        public string StylistName { get; set; }

        public Guid ClientId { get; set; }

        public Guid StylistId { get; set; }

        public string? Text { get; set; }

        public int Score { get; set; }

    }
}
