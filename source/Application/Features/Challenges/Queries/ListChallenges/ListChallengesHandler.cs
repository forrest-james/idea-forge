using Application.Features.Challenges.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Challenges.Queries.ListChallenges
{
    public sealed class ListChallengesHandler : IRequestHandler<ListChallengesQuery, IReadOnlyList<ChallengeListItemDto>>
    {
        private readonly IAppDbContext _dbContext;

        public ListChallengesHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<IReadOnlyList<ChallengeListItemDto>> Handle(ListChallengesQuery request, CancellationToken cancellationToken)
        {
            var items = await _dbContext.Challenges
                .AsNoTracking()
                .Include(c => c.AppIdea)
                .Include(c => c.Palette)
                .OrderByDescending(c => c.CreatedAtUtc)
                .Select(c => new ChallengeListItemDto(                
                    Id: c.Id,
                    Name: c.Name,
                    AppType: c.AppIdea.Type.ToString(),
                    AppCategory: c.AppIdea.Category.ToString(),
                    PrimaryColor: c.Palette.PrimaryColor.ToString(),
                    SecondaryColor: c.Palette.SecondaryColor.ToString(),
                    AccentColor: c.Palette.AccentColor.ToString()
                ))
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}