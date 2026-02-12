using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Palettes.Queries.GetPaletteIdsInUse
{
    public sealed class GetPaletteIdsInUseHandler : IRequestHandler<GetPaletteIdsInUseQuery, HashSet<Guid>>
    {
        private readonly IAppDbContext _dbContext;
        public GetPaletteIdsInUseHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<HashSet<Guid>> Handle(GetPaletteIdsInUseQuery request, CancellationToken cancellationToken)
        {
            var ids = await _dbContext.Challenges
                .AsNoTracking()
                .Select(c => EF.Property<Guid>(c, "PaletteId"))
                .Distinct()
                .ToListAsync(cancellationToken);

            return ids.ToHashSet();
        }
    }
}