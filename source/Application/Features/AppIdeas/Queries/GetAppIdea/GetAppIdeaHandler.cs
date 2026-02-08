using Application.Features.AppIdeas.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AppIdeas.Queries.GetAppIdea
{
    public sealed class GetAppIdeaHandler : IRequestHandler<GetAppIdeaQuery, AppIdeaDto?>
    {
        private readonly IAppDbContext _dbContext;
        public GetAppIdeaHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<AppIdeaDto?> Handle(GetAppIdeaQuery request, CancellationToken cancellationToken)
        {
            var raw = await _dbContext.AppIdeas
                .AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new { a.Id, a.Type, a.Category, a.Description })
                .FirstOrDefaultAsync(cancellationToken);

            return raw is null
                ? null
                : new AppIdeaDto
                (
                    raw.Id,
                    raw.Type.ToString(),
                    raw.Category.ToString(),
                    raw.Description
                );
        }
    }
}