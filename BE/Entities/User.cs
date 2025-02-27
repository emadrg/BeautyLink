using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime CreatedDate { get; set;}
        public DateTime LastModifiedDate { get; set; }
    }
}
