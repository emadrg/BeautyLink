using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class UserStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
