﻿using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ServiceStylist> ServiceStylists { get; set; } = new List<ServiceStylist>();
}
