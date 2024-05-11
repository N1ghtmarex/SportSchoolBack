using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Domain.Interfaces;
using Domain.Entities;

namespace Domain;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Coach> Coachs { get; set; } = null!;
    public DbSet<Section> Sections { get; set; } = null!;
    public DbSet<Sport> Sports { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<SectionEvent> SectionEvents { get; set; }
    public DbSet<IndividualEvent> IndividualEvents { get; set; }

    public override int SaveChanges()
    {
        UpdateTrackDate();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTrackDate();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateTrackDate();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        UpdateTrackDate();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateTrackDate()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: IHasTrackDateAttribute, State: EntityState.Added or EntityState.Modified });

        foreach (var entityEntry in entries)
        {
            ((IHasTrackDateAttribute)entityEntry.Entity).DateModified = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((IHasTrackDateAttribute)entityEntry.Entity).DateCreated = DateTime.UtcNow;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly);

        //Настройка связей для секции
        //Отношения между клиентом и секцией
        modelBuilder.Entity<Section>()
            .HasMany(s => s.Client)
            .WithMany(c => c.Section)
            .UsingEntity(j => j.ToTable("ClientSection"));

        //Отношения между тренером и секцией
        modelBuilder.Entity<Section>()
            .HasOne(s => s.Coach)
            .WithMany(c => c.Section);

        //Отношения между залом и секцией
        modelBuilder.Entity<Section>()
            .HasOne(s => s.Room)
            .WithMany(r => r.Section);

        //Отношения между спортом и секцией
        modelBuilder.Entity<Section>()
            .HasOne(s => s.Sport)
            .WithMany(s => s.Section);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}