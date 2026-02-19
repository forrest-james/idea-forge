using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Challenges.Queries.GetChallengeIdsWithSubmissions
{
    public sealed class GetChallengeIdsWithSubmissionsHandler : IRequestHandler<GetChallengeIdsWithSubmissionsQuery, HashSet<Guid>>
    {
        private readonly IAppDbContext _dbContext;
        public GetChallengeIdsWithSubmissionsHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<HashSet<Guid>> Handle(GetChallengeIdsWithSubmissionsQuery request, CancellationToken cancellationToken)
        {
            var ids = await _dbContext.Submissions
                .AsNoTracking()
                .Select(s => EF.Property<Guid>(s, "ChallengeId"))
                .Distinct()
                .ToListAsync(cancellationToken);

            return ids.ToHashSet();
        }
    }
}