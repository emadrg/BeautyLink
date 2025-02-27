using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class SalonStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Salon> Salons { get; set; } = new List<Salon>();
}
