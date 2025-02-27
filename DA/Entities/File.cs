using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class File
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public string Extesion { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Salon> Salons { get; set; } = new List<Salon>();

    public virtual ICollection<Salon> SalonsNavigation { get; set; } = new List<Salon>();
}
