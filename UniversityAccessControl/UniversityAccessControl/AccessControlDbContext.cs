using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Models;

namespace UniversityAccessControl;

public class AccessControlDbContext : DbContext
{
    public AccessControlDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<AccessLogEntry> AccessLogEntries => Set<AccessLogEntry>();
    public virtual DbSet<Area> Areas => Set<Area>();
    public virtual DbSet<Group> Groups => Set<Group>();
    public virtual DbSet<Passage> Passages => Set<Passage>();
    public virtual DbSet<Rule> Rules => Set<Rule>();
    public virtual DbSet<Subject> Subjects => Set<Subject>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Rule>()
            .HasOne(e => e.Subject)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<Rule>()
            .HasOne(e => e.Group)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<Rule>()
            .HasOne(e => e.Passage)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<Rule>()
            .HasOne(e => e.Area)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}