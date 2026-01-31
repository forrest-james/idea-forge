using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<AppIdea> AppIdeas { get; }
        DbSet<Palette> Palettes { get; }
        DbSet<Challenge> Challenges { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}