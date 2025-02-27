using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Appointment
{
    public int Id { get; set; }

    public Guid ClientId { get; set; }

    public int StatusId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual AppointmentStatus Status { get; set; } = null!;

    public virtual ICollection<ServiceStylist> ServiceStylists { get; set; } = new List<ServiceStylist>();
}
