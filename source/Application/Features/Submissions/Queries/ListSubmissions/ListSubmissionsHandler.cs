using Application.Features.Submissions.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Submissions.Queries.ListSubmissions
{
    public sealed class ListSubmissionsHandler : IRequestHandler<ListSubmissionsQuery, IReadOnlyList<SubmissionListItemDto>>
    {
        private readonly IAppDbContext _dbContext;
        public ListSubmissionsHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<IReadOnlyList<SubmissionListItemDto>> Handle(ListSubmissionsQuery request, CancellationToken cancellationToken)
        {
            var raw = await _dbContext.Submissions
                .AsNoTracking()
                .Select(s => new
                {
                    s.Id,
                    ChallengeName = s.Challenge.Name,
                    s.Description,
                    ImageCount = s.Images.Count
                })
                .ToListAsync(cancellationToken);

            var items = raw
                .OrderBy(x => x.ChallengeName)
                .ThenBy(x => x.Id)
                .Select(x => new SubmissionListItemDto(
                    Id: x.Id,
                    ChallengeName: x.ChallengeName,
                    DescriptionPreview: MakePreview(x.Description, 140),
                    ImageCount: x.ImageCount
                ))
                .ToList();

            return items;
        }
        private static string MakePreview(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            var trimmed = text.Trim();
            return trimmed.Length <= maxLength
                ? trimmed
                : trimmed.Substring(0, maxLength).TrimEnd() + "…";
        }
    }
}