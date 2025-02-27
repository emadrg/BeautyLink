using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class UnavailableTime
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Reason { get; set; }

    public Guid StylistId { get; set; }

    public virtual Stylist Stylist { get; set; } = null!;
}
