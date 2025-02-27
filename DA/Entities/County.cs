using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class County
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual ICollection<Salon> Salons { get; set; } = new List<Salon>();
}
