using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Picture
{
    public int Id { get; set; }

    public byte[] FileContent { get; set; } = null!;

    public Guid StylistId { get; set; }

    public virtual Stylist Stylist { get; set; } = null!;
}
