using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Models;

public partial class CarInspectionDbContext : DbContext
{
    public CarInspectionDbContext() { }

    public CarInspectionDbContext(DbContextOptions<CarInspectionDbContext> options)
        : base(options) { }

    public virtual DbSet<InspectionRecord> InspectionRecords { get; set; }

    public virtual DbSet<InspectionStation> InspectionStations { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InspectionRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Inspecti__FBDF78C9C26DF7AA");

            entity.Property(e => e.RecordId).ValueGeneratedNever().HasColumnName("RecordID");
            entity
                .Property(e => e.Co2emission)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("CO2Emission");
            entity
                .Property(e => e.Hcemission)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("HCEmission");
            entity.Property(e => e.InspectionDate).HasColumnType("datetime");
            entity.Property(e => e.InspectorId).HasColumnName("InspectorID");
            entity.Property(e => e.Result).HasMaxLength(10);
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");

            entity
                .HasOne(d => d.Inspector)
                .WithMany(p => p.InspectionRecords)
                .HasForeignKey(d => d.InspectorId)
                .HasConstraintName("FK__Inspectio__Inspe__3F466844");

            entity
                .HasOne(d => d.Station)
                .WithMany(p => p.InspectionRecords)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inspectio__Stati__3E52440B");

            entity
                .HasOne(d => d.Vehicle)
                .WithMany(p => p.InspectionRecords)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inspectio__Vehic__3D5E1FD2");
        });

        modelBuilder.Entity<InspectionStation>(entity =>
        {
            entity.HasKey(e => e.StationId).HasName("PK__Inspecti__E0D8A6DD4FD57C1C");

            entity.Property(e => e.StationId).ValueGeneratedNever().HasColumnName("StationID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Logs__5E5499A888C4DF1B");

            entity.Property(e => e.LogId).ValueGeneratedNever().HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Logs__UserID__4222D4EF");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E321B74F842");

            entity
                .Property(e => e.NotificationId)
                .ValueGeneratedNever()
                .HasColumnName("NotificationID");
            entity.Property(e => e.SentDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__44FF419A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC2164E8A6");

            entity.Property(e => e.UserId).ValueGeneratedNever().HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicles__476B54B2D5E47F55");

            entity.Property(e => e.VehicleId).ValueGeneratedNever().HasColumnName("VehicleID");
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.EngineNumber).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.PlateNumber).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
