using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Q2.Models;

public partial class PePrn25sprB5Context : DbContext
{
    public PePrn25sprB5Context()
    {
    }

    public PePrn25sprB5Context(DbContextOptions<PePrn25sprB5Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("MyCnn"));
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BED642CB22E");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.ToTable("Instructor");

            entity.Property(e => e.Fullname).HasMaxLength(200);

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.Department)
                .HasConstraintName("FK__Instructo__Depar__3C69FB99");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.MajorCode).HasName("PK__Major__64E58F952B2C8F3D");

            entity.ToTable("Major");

            entity.Property(e => e.MajorCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.MajorName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Student");

            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.Major)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.StudentId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.MajorNavigation).WithMany()
                .HasForeignKey(d => d.Major)
                .HasConstraintName("FK__Student__Major__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
