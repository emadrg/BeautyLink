using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Direction
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
