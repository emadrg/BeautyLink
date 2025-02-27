using System;
using System.Collections.Generic;

namespace DA.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public string Password { get; set; } = null!;

    public byte RoleId { get; set; }

    public int StatusId { get; set; }

    public int? ProfilePictureId { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseLastModifiedByNavigation { get; set; } = new List<User>();

    public virtual User? LastModifiedByNavigation { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual AppFile? ProfilePicture { get; set; }

    public virtual ICollection<ReviewClientSalon> ReviewClientSalons { get; set; } = new List<ReviewClientSalon>();

    public virtual ICollection<ReviewClientStylist> ReviewClientStylists { get; set; } = new List<ReviewClientStylist>();

    public virtual ICollection<ReviewStylistClient> ReviewStylistClients { get; set; } = new List<ReviewStylistClient>();

    public virtual Role Role { get; set; } = null!;

    public virtual UserStatus Status { get; set; } = null!;

    public virtual ICollection<Stylist> Stylists { get; set; } = new List<Stylist>();
}
