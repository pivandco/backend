using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Models;

namespace UniversityAccessControl;

public class AccessControlDbContext : DbContext
{
    public virtual DbSet<AccessLogEntry> AccessLogEntries => Set<AccessLogEntry>();
    public virtual DbSet<Area> Areas => Set<Area>();
    public virtual DbSet<Group> Groups => Set<Group>();
    public virtual DbSet<Passage> Passages => Set<Passage>();
    public virtual DbSet<Rule> Rules => Set<Rule>();
    public virtual DbSet<Subject> Subjects => Set<Subject>();
}