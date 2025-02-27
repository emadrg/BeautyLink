using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class ServiceStylist
{
    public int ServiceId { get; set; }

    public Guid StylistId { get; set; }

    public int DurationMinutes { get; set; }

    public decimal Price { get; set; }

    public int Id { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual Stylist Stylist { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
