using System;
using System.Collections.Generic;
using DA.Entities;
using Microsoft.EntityFrameworkCore;

namespace DA.Context;

public partial class SalonContext : DbContext
{
    public SalonContext()
    {
    }

    public SalonContext(DbContextOptions<SalonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppFile> AppFiles { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<County> Counties { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<ReviewClientSalon> ReviewClientSalons { get; set; }

    public virtual DbSet<ReviewClientStylist> ReviewClientStylists { get; set; }

    public virtual DbSet<ReviewStylistClient> ReviewStylistClients { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salon> Salons { get; set; }

    public virtual DbSet<SalonStatus> SalonStatuses { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceDetailsVw> ServiceDetailsVws { get; set; }

    public virtual DbSet<ServiceStylist> ServiceStylists { get; set; }

    public virtual DbSet<Stylist> Stylists { get; set; }

    public virtual DbSet<UnavailableTime> UnavailableTimes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    public virtual DbSet<WeekDay> WeekDays { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_file");

            entity.ToTable("AppFile");

            entity.Property(e => e.Extesion).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Path).HasMaxLength(1000);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_appointment");

            entity.ToTable("Appointment");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appointment_service_client");

            entity.HasOne(d => d.Status).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appointment_status");

            entity.HasMany(d => d.ServiceStylists).WithMany(p => p.Appointments)
                .UsingEntity<Dictionary<string, object>>(
                    "AppointmentStylistService",
                    r => r.HasOne<ServiceStylist>().WithMany()
                        .HasForeignKey("ServiceStylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_appointment_stylist_service_service_stylist"),
                    l => l.HasOne<Appointment>().WithMany()
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_appointment_stylist_service_appointment"),
                    j =>
                    {
                        j.HasKey("AppointmentId", "ServiceStylistId").HasName("pk_appointment_stylist_service");
                        j.ToTable("AppointmentStylistService");
                    });
        });

        modelBuilder.Entity<AppointmentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_status");

            entity.ToTable("AppointmentStatus");

            entity.Property(e => e.Name).HasMaxLength(15);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_city");

            entity.ToTable("City");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.County).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_city_county");
        });

        modelBuilder.Entity<County>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_county");

            entity.ToTable("County");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_table");

            entity.ToTable("Log");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(1000);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_log_user");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_manager");

            entity.ToTable("Manager");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Salon).WithMany(p => p.Managers)
                .HasForeignKey(d => d.SalonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_manager_salon");

            entity.HasOne(d => d.User).WithMany(p => p.Managers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_manager_user");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_notification");

            entity.ToTable("Notification");

            entity.Property(e => e.ReadDate).HasColumnType("datetime");
            entity.Property(e => e.SendDate).HasColumnType("datetime");
            entity.Property(e => e.Text).HasMaxLength(100);

            entity.HasOne(d => d.Receiver).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_notification_receiver");
        });

        modelBuilder.Entity<ReviewClientSalon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fk_review_client_salon");

            entity.ToTable("ReviewClientSalon");

            entity.HasIndex(e => new { e.ClientId, e.SalonId }, "review_client_salon_uq").IsUnique();

            entity.Property(e => e.Text).HasMaxLength(1000);

            entity.HasOne(d => d.Client).WithMany(p => p.ReviewClientSalons)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_client_salon_client");

            entity.HasOne(d => d.Salon).WithMany(p => p.ReviewClientSalons)
                .HasForeignKey(d => d.SalonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_client_salon_salon");
        });

        modelBuilder.Entity<ReviewClientStylist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fk_review_client_stylist");

            entity.ToTable("ReviewClientStylist");

            entity.HasIndex(e => new { e.ClientId, e.StylistId }, "review_client_stylist_uq").IsUnique();

            entity.Property(e => e.Text).HasMaxLength(1000);

            entity.HasOne(d => d.Client).WithMany(p => p.ReviewClientStylists)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_client_stylist_client");

            entity.HasOne(d => d.Stylist).WithMany(p => p.ReviewClientStylists)
                .HasForeignKey(d => d.StylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_client_stylist_stylist");
        });

        modelBuilder.Entity<ReviewStylistClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fk_review_stylist_client");

            entity.ToTable("ReviewStylistClient");

            entity.HasIndex(e => new { e.StylistId, e.ClientId }, "review_stylist_client_uq").IsUnique();

            entity.Property(e => e.Text).HasMaxLength(1000);

            entity.HasOne(d => d.Client).WithMany(p => p.ReviewStylistClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_stylist_client_client");

            entity.HasOne(d => d.Stylist).WithMany(p => p.ReviewStylistClients)
                .HasForeignKey(d => d.StylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_stylist_client_stylist");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_role");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Salon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_salon");

            entity.ToTable("Salon");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Latitude).HasColumnType("float");
            entity.Property(e => e.Longitude).HasColumnType("float");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.City).WithMany(p => p.Salons)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_salon_city");

            entity.HasOne(d => d.County).WithMany(p => p.Salons)
                .HasForeignKey(d => d.CountyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_salon_county");

            entity.HasOne(d => d.Status).WithMany(p => p.Salons)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_salon_status");

            entity.HasMany(d => d.Files).WithMany(p => p.Salons)
                .UsingEntity<Dictionary<string, object>>(
                    "SalonPicture",
                    r => r.HasOne<AppFile>().WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_salon_picture_file"),
                    l => l.HasOne<Salon>().WithMany()
                        .HasForeignKey("SalonId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_salon_picture_salon"),
                    j =>
                    {
                        j.HasKey("SalonId", "FileId").HasName("pk_salon_picture");
                        j.ToTable("SalonPicture");
                    });

            entity.HasMany(d => d.FilesNavigation).WithMany(p => p.SalonsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "SalonRegistrationDocument",
                    r => r.HasOne<AppFile>().WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_salon_registration_document_file"),
                    l => l.HasOne<Salon>().WithMany()
                        .HasForeignKey("SalonId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_salon_registration_document_salon"),
                    j =>
                    {
                        j.HasKey("SalonId", "FileId").HasName("pk_salon_registration_document");
                        j.ToTable("SalonRegistrationDocument");
                    });
        });

        modelBuilder.Entity<SalonStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_salon_status");

            entity.ToTable("SalonStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_schedule");

            entity.ToTable("Schedule");

            entity.HasOne(d => d.Stylist).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.StylistId)
                .HasConstraintName("fk_schedule_stylist");

            entity.HasOne(d => d.WeekDay).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.WeekDayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_schedule_week_day");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_service");

            entity.ToTable("Service");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ServiceDetailsVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ServiceDetailsVw");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CountyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SalonName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.StylistName).HasMaxLength(101);
        });

        modelBuilder.Entity<ServiceStylist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_service_stylist");

            entity.ToTable("ServiceStylist");

            entity.HasIndex(e => new { e.ServiceId, e.StylistId }, "uq_service_stylist").IsUnique();

            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceStylists)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_service_stylist_service");

            entity.HasOne(d => d.Stylist).WithMany(p => p.ServiceStylists)
                .HasForeignKey(d => d.StylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_service_stylist_stylist");
        });

        modelBuilder.Entity<Stylist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_stylist");

            entity.ToTable("Stylist");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SocialMediaLink).HasMaxLength(500);

            entity.HasOne(d => d.Salon).WithMany(p => p.Stylists)
                .HasForeignKey(d => d.SalonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_stylist_salon");

            entity.HasOne(d => d.User).WithMany(p => p.Stylists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_stylist_user");
        });

        modelBuilder.Entity<UnavailableTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_unavailableTime");

            entity.ToTable("UnavailableTime");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Stylist).WithMany(p => p.UnavailableTimes)
                .HasForeignKey(d => d.StylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_unavailableTime_stylist");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_user");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_user_created_by");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.InverseLastModifiedByNavigation)
                .HasForeignKey(d => d.LastModifiedBy)
                .HasConstraintName("fk_user_last_modified_by");

            entity.HasOne(d => d.ProfilePicture).WithMany(p => p.Users)
                .HasForeignKey(d => d.ProfilePictureId)
                .HasConstraintName("fk_user_file");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_status");
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_user_status");

            entity.ToTable("UserStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WeekDay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_week_day");

            entity.ToTable("WeekDay");

            entity.Property(e => e.Name).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
