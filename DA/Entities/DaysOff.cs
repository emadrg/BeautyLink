using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class DaysOff
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string? Reason { get; set; }

    public virtual ICollection<Stylist> Stylists { get; set; } = new List<Stylist>();
}
