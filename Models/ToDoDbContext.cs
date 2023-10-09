using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace to_do_api.Models;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activities> Activities { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=host.docker.internal;port=3306;user=root;password=password;database=MyDatabase", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.0.3-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Activities>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PRIMARY");

            entity.ToTable("activities");

            entity.HasIndex(e => e.UserId, "userID");

            entity.Property(e => e.ActivityId)
                .HasColumnType("int(11)")
                .HasColumnName("activityID");
            entity.Property(e => e.ActivitiesTime)
                .HasColumnType("datetime")
                .HasColumnName("activitiesTime");
            entity.Property(e => e.ActivityName)
                .HasMaxLength(255)
                .HasColumnName("activityName");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Activities)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("activities_ibfk_1");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userID");
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .HasColumnName("salt");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .HasColumnName("userPassword");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
