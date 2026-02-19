using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Challenges.Commands.DeleteChallenge
{
    public sealed class DeleteChallengeHandler : IRequestHandler<DeleteChallengeCommand>
    {
        private readonly IAppDbContext _dbContext;
        public DeleteChallengeHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Handle(DeleteChallengeCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new InvalidOperationException("Id is required.");

            var hasSubmissions = await _dbContext.Submissions
                .AsNoTracking()
                .AnyAsync(s => EF.Property<Guid>(s, "ChallengeId") == request.Id, cancellationToken);

            if (hasSubmissions)
                throw new InvalidOperationException("Cannot delete challenge because it has at least one submission.");

            var challenge = await _dbContext.Challenges.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (challenge is null)
                return;

            _dbContext.Challenges.Remove(challenge);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}