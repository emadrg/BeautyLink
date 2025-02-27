using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Schedule
{
    public int Id { get; set; }

    public Guid? StylistId { get; set; }

    public int WeekDayId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual Stylist? Stylist { get; set; }

    public virtual WeekDay WeekDay { get; set; } = null!;
}
