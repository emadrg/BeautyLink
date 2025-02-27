using DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string jwt { get; set; }
        public UserDetailsDTO userDetails { get; set; }
    }
}
