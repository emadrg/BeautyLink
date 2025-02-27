using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class WeekDay
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
