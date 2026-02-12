using Application.Features.Submissions.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Submissions.Queries.ListSubmissionsByChallenge
{
    public sealed class ListSubmissionsByChallengeHandler : IRequestHandler<ListSubmissionsByChallengeQuery, IReadOnlyList<SubmissionListItemDto>>
    {
        private readonly IAppDbContext _dbContext;
        public ListSubmissionsByChallengeHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<IReadOnlyList<SubmissionListItemDto>> Handle(ListSubmissionsByChallengeQuery request, CancellationToken cancellationToken)
        {
            var raw = await _dbContext.Submissions
                .AsNoTracking()
                .Where(s => EF.Property<Guid>(s, "ChallengeId") == request.ChallengeId)
                .Select(s => new                
                {
                    s.Id,
                    s.Description,
                    ImageCount = s.Images.Count
                })
                .ToListAsync(cancellationToken);

            return raw
                .OrderByDescending(x => x.Id)
                .Select(x => new SubmissionListItemDto
                (
                    Id: x.Id,
                    ChallengeName: string.Empty,
                    DescriptionPreview: MakePreview(x.Description, 120),
                    ImageCount: x.ImageCount
                ))
                .ToList();
        }

        private static string MakePreview(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var trimmed = text.Trim();
            return trimmed.Length <= maxLength
                ? trimmed
                : trimmed.Substring(0, maxLength).TrimEnd() + "…";
        }
    }
}