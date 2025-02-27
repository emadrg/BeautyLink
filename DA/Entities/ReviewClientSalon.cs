using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class ReviewClientSalon
{
    public int Id { get; set; }

    public Guid ClientId { get; set; }

    public int SalonId { get; set; }

    public string? Text { get; set; }

    public int Score { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual Salon Salon { get; set; } = null!;
}
