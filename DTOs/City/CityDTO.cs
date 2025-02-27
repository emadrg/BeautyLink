using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.City
{
    public class CityDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int CountyId { get; set; }

    }
    
}
