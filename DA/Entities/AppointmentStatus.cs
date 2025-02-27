using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class AppointmentStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
