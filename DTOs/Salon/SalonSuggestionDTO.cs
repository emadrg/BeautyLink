using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Salon
{
    public class SalonSuggestionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string Address { get; set; }

        public int ReviewsNumber { get; set; } = 0;

        public double AvgScore { get; set; } = 0;

    }
}
