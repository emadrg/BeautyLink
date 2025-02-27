using DA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Manager
{
    public class ManagerDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int SalonId { get; set; }

    }
}
