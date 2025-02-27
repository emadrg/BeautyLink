using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Salon
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CountyId { get; set; }

    public int CityId { get; set; }

    public string? Address { get; set; }

    public int StatusId { get; set; }

    public float Latitude { get; set; }

    public float Longitude { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual County County { get; set; } = null!;

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();

    public virtual ICollection<ReviewClientSalon> ReviewClientSalons { get; set; } = new List<ReviewClientSalon>();

    public virtual SalonStatus Status { get; set; } = null!;

    public virtual ICollection<Stylist> Stylists { get; set; } = new List<Stylist>();

    public virtual ICollection<AppFile> Files { get; set; } = new List<AppFile>();

    public virtual ICollection<AppFile> FilesNavigation { get; set; } = new List<AppFile>();
}
