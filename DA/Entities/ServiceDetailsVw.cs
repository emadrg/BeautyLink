using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class ServiceDetailsVw
{
    public int SalonId { get; set; }

    public string SalonName { get; set; } = null!;

    public string? Address { get; set; }

    public string CityName { get; set; } = null!;

    public int CityId { get; set; }

    public string CountyName { get; set; } = null!;

    public int CountyId { get; set; }

    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public Guid StylistId { get; set; }

    public string StylistName { get; set; } = null!;
}
