using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Allocation> Allocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Department).IsRequired().HasMaxLength(50);
            });

            // Configuração da entidade Asset
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.SerialNumber).IsUnique();
                entity.Property(e => e.Value).HasColumnType("decimal(18,2)");
            });

            // Configuração da entidade Allocation
            modelBuilder.Entity<Allocation>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Asset)
                    .WithMany(a => a.Allocations)
                    .HasForeignKey(e => e.AssetId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Allocations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Notes).HasMaxLength(500);
            });
        }
    }
}