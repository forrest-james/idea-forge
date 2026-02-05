using Application.Features.Submissions.DTOs;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Submissions.Queries.GetSubmission
{
    public sealed class GetSubmissionHandler : IRequestHandler<GetSubmissionQuery, SubmissionDto?>
    {
        private readonly IAppDbContext _dbContext;

        public GetSubmissionHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<SubmissionDto?> Handle(GetSubmissionQuery request, CancellationToken cancellationToken)
        {
            var submission = await _dbContext.Submissions
                .AsNoTracking()
                .Include(s => s.Challenge)
                .Include(s => s.Images)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (submission is null)
                return null;

            var images = submission.Images
                .OrderBy(i => i.Order)
                .Select(i => new SubmissionImageDto(
                    Id: i.Id,
                    ImageUrl: i.Url.ToString(),
                    Order: i.Order
                ))
                .ToList();

            return new SubmissionDto(
                Id: submission.Id,
                Description: submission.Description,
                Challenge: new ChallengeSummaryDto(
                    Id: submission.Challenge.Id,
                    Name: submission.Challenge.Name
                ),
                Images: images
            );
        }
    }
}