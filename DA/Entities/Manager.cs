using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Manager
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int SalonId { get; set; }

    public virtual Salon Salon { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
