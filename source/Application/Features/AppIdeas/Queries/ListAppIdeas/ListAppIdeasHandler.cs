using Application.Features.AppIdeas.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AppIdeas.Queries.ListAppIdeas
{
    public sealed class ListAppIdeasHandler : IRequestHandler<ListAppIdeasQuery, IReadOnlyList<AppIdeaDto>>
    {
        private readonly IAppDbContext _dbContext;

        public ListAppIdeasHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<IReadOnlyList<AppIdeaDto>> Handle(ListAppIdeasQuery request, CancellationToken cancellationToken)
        {
            var raw = await _dbContext.AppIdeas
                .AsNoTracking()
                .Select(a => new
                {
                    a.Id,
                    a.Type,
                    a.Category,
                    a.Description
                })
                .ToListAsync(cancellationToken);

            return raw
                .OrderBy(a => a.Category)
                .ThenBy(a => a.Type)
                .ThenBy(a => a.Id)
                .Select(a => new AppIdeaDto
                (
                    Id: a.Id,
                    Type: a.Type.ToString(),
                    Category: a.Category.ToString(),
                    Description: a.Description
                ))
                .ToList();
        }
    }
}