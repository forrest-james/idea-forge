using Data.Converters;
using Data.Models;
using Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.EF
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppIdea> AppIdeas => Set<AppIdea>();
        public DbSet<Palette> Palettes => Set<Palette>();
        public DbSet<Challenge> Challenges => Set<Challenge>();
        public DbSet<Submission> Submissions => Set<Submission>();
        public DbSet<Image> Images => Set<Image>();

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
                entity.Navigation(e => e.Images)
                    .HasField("_images")
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
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

            modelBuilder.Entity<Submission>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Image>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
        }
    }
}