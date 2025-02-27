using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Log
{
    public int Id { get; set; }

    public byte LogLevel { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public string StackTrace { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual User? CreatedByNavigation { get; set; }
}
