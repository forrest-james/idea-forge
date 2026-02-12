using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AppIdeas.Queries.GetAppIdeaIdsInUse
{
    public sealed class GetAppIdeaIdsInUseHandler : IRequestHandler<GetAppIdeaIdsInUseQuery, HashSet<Guid>>
    {
        private readonly IAppDbContext _dbContext;
        public GetAppIdeaIdsInUseHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<HashSet<Guid>> Handle(GetAppIdeaIdsInUseQuery request, CancellationToken cancellationToken)
        {
            var ids = await _dbContext.Challenges
                .AsNoTracking()
                .Select(c => EF.Property<Guid>(c, "AppIdeaId"))
                .Distinct()
                .ToListAsync(cancellationToken);

            return ids.ToHashSet();
        }
    }
}