using DTOs.Salon;
using DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Manager
{
    public class CreateManagerWithSalonDTO
    {

        public RegisterManagerDTO Manager { get; set; } = null!;

        public CreateSalonDTO? Salon { get; set; }
    }
}
