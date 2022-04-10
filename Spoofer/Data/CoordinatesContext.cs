using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Spoofer.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spoofer.Data
{
    public partial class CoordinatesContext : DbContext
    {
        public CoordinatesContext()
        {
        }

        public CoordinatesContext(DbContextOptions options)
            : base(options)
        {
        }
        public virtual DbSet<Coordinates> Coordinates { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Coordinates;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coordinates>(entity =>
            {
                entity.HasKey(e => e.CoorfianteId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.CoorfianteId).HasMaxLength(50);

                entity.Property(e => e.Name)
                    
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Coordinates)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Coordinates_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
