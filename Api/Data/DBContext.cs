using Logic;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Message> message { get; set; }  
    public DbSet<ImageApi> imageapi { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasKey(m => m.messageid);

         modelBuilder.Entity<ImageApi>()
            .HasKey(i => i.id);
    }
}
