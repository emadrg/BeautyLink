using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Salon
{
    public class CreateSalonDTO
    {
        public string Name { get; set; }

        public int CountyId { get; set; }

        public int CityId { get; set; }

        public string Address { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
    }
}
