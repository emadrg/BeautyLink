using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class ReviewClientStylist
{
    public int Id { get; set; }

    public Guid ClientId { get; set; }

    public Guid StylistId { get; set; }

    public string? Text { get; set; }

    public int Score { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual Stylist Stylist { get; set; } = null!;
}
