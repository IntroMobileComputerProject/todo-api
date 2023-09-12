﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using to_do_api.Models;

#nullable disable

namespace to_do_api.Migrations
{
    [DbContext(typeof(ToDoDbContext))]
    partial class ToDoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("to_do_api.Models.Activities", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("activityID");

                    b.Property<DateTime>("ActivitiesTime")
                        .HasColumnType("datetime")
                        .HasColumnName("activitiesTime");

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("activityName");

                    b.Property<int?>("UserId")
                        .HasColumnType("int(11)")
                        .HasColumnName("userID");

                    b.HasKey("ActivityId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "UserId" }, "userID");

                    b.ToTable("activities", (string)null);
                });

            modelBuilder.Entity("to_do_api.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("userID");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("salt");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("userName");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("userPassword");

                    b.HasKey("UserId")
                        .HasName("PRIMARY");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("to_do_api.Models.Activities", b =>
                {
                    b.HasOne("to_do_api.Models.Users", "User")
                        .WithMany("Activities")
                        .HasForeignKey("UserId")
                        .HasConstraintName("activities_ibfk_1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("to_do_api.Models.Users", b =>
                {
                    b.Navigation("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}
