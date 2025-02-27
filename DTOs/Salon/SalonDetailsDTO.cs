using DTOs.Service;
using DTOs.ServiceStylist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Salon
{
    public class SalonDetailsDTO
    {
        public int SalonId { get; set; }

        public string SalonName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int CityId { get; set; }

        public string County { get; set; }

        public int CountyId { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public List<ServiceStylistDTO> ServiceStylists { get; set; }
    }
}
