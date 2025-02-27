using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Entities
{
    public partial class Log
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte LogLevel { get; set; }
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }
        public Guid? UserId { get; set; }
    }
}
