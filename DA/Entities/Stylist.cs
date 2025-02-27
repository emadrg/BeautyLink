using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Stylist
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int SalonId { get; set; }

    public string? SocialMediaLink { get; set; }

    public virtual ICollection<ReviewClientStylist> ReviewClientStylists { get; set; } = new List<ReviewClientStylist>();

    public virtual ICollection<ReviewStylistClient> ReviewStylistClients { get; set; } = new List<ReviewStylistClient>();

    public virtual Salon Salon { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<ServiceStylist> ServiceStylists { get; set; } = new List<ServiceStylist>();

    public virtual ICollection<UnavailableTime> UnavailableTimes { get; set; } = new List<UnavailableTime>();

    public virtual User User { get; set; } = null!;
}
