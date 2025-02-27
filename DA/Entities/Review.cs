using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Review
{
    public int Id { get; set; }

    public int DirectionId { get; set; }

    public Guid ClientId { get; set; }

    public Guid? StylistId { get; set; }

    public int? SalonId { get; set; }

    public string? Text { get; set; }

    public int Score { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual Direction Direction { get; set; } = null!;

    public virtual Salon? Salon { get; set; }

    public virtual User? Stylist { get; set; }
}
