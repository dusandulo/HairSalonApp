using Microsoft.EntityFrameworkCore;
using HairSalon.API.Models;

namespace HairSalon.API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Client)
            .WithMany()
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Stylist)
            .WithMany()
            .HasForeignKey(a => a.StylistId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
