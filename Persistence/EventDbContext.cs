using Microsoft.EntityFrameworkCore;
using SistemaAPIRest.Entities;

namespace SistemaAPIRest.Persistence;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<EventSpeaker> Speakers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(e =>
        {
            e.HasKey(e => e.Id);
            e.Property(e => e.Title).IsRequired(false);
            e.Property(e => e.Description)
             .HasMaxLength(200)
             .HasColumnType("varchar(200)");
            e.Property(e => e.CreatedDate)
             .HasColumnName("Created_Date");
            e.Property(e => e.EndDate)
             .HasColumnName("End_Date");
            e.HasMany(es => es.Speakers)
             .WithOne()
             .HasForeignKey(es => es.EventSpeakerId);
        });

        modelBuilder.Entity<EventSpeaker>(es =>
        {
            es.HasKey(es => es.Id);
        });
    }
}
