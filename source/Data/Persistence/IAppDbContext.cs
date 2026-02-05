using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public interface IAppDbContext
    {
        DbSet<AppIdea> AppIdeas { get; }
        DbSet<Palette> Palettes { get; }
        DbSet<Challenge> Challenges { get; }
        DbSet<Submission> Submissions { get; }
        DbSet<Image> Images { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}