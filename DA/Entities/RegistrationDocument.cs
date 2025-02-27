using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class RegistrationDocument
{
    public int Id { get; set; }

    public int SalonId { get; set; }

    public byte[] FileContent { get; set; } = null!;

    public virtual Salon Salon { get; set; } = null!;
}
