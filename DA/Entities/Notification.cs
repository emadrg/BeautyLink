using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public Guid ReceiverId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime SendDate { get; set; }

    public DateTime? ReadDate { get; set; }

    public virtual User Receiver { get; set; } = null!;
}
