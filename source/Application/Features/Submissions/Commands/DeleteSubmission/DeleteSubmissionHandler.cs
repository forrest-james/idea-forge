using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Submissions.Commands.DeleteSubmission
{
    public sealed class DeleteSubmissionHandler : IRequestHandler<DeleteSubmissionCommand>
    {
        private readonly IAppDbContext _dbContext;
        public DeleteSubmissionHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Handle(DeleteSubmissionCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new InvalidOperationException("Id is required.");

            var hasImages = await _dbContext.Images
                .AsNoTracking()
                .AnyAsync(i => i.SubmissionId == request.Id, cancellationToken);

            if (hasImages)
                throw new InvalidOperationException("Cannot delete submission because it has images.");

            var submission = await _dbContext.Submissions
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (submission is null)
                return;

            _dbContext.Submissions.Remove(submission);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}