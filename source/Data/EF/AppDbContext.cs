using Data.Converters;
using Data.Models;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Data.EF
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<AppIdea> AppIdeas => Set<AppIdea>();
        public DbSet<Palette> Palettes => Set<Palette>();
        public DbSet<Challenge> Challenges => Set<Challenge>();        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Palette>(entity =>
            {
                entity.Property(e => e.PrimaryColor)
                    .HasConversion(new HexColorValueConverter())
                    .HasMaxLength(7)
                    .IsRequired();
                entity.Property(e => e.SecondaryColor)
                    .HasConversion(new HexColorValueConverter())
                    .HasMaxLength(7)
                    .IsRequired();
                entity.Property(e => e.AccentColor)
                    .HasConversion(new HexColorValueConverter())
                    .HasMaxLength(7)
                    .IsRequired();
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasMany(e => e.Images)
                      .WithOne()
                      .HasForeignKey(i => i.SubmissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.Id);                                                                         

                entity.Property(e => e.SubmissionId).IsRequired();
                entity.Property(e => e.Order).IsRequired();

                entity.OwnsOne(e => e.Url, url =>
                {
                    url.Property(u => u.Value)
                        .HasColumnName("Url")
                        .IsRequired();
                });
            });        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdeaForge_Local;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");
        }
    }
}