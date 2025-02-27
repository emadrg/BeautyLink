using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class City
{
    public int Id { get; set; }

    public int CountyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual County County { get; set; } = null!;

    public virtual ICollection<Salon> Salons { get; set; } = new List<Salon>();
}
