using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Challenges.Commands.UpdateChallengeName
{
    public sealed class UpdateChallengeNameHandler : IRequestHandler<UpdateChallengeNameCommand>
    {
        private readonly IAppDbContext _dbContext;
        public UpdateChallengeNameHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Handle(UpdateChallengeNameCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new InvalidOperationException("Id is required.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidOperationException("Name is required.");

            var challenge = await _dbContext.Challenges.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (challenge == null)
                throw new InvalidOperationException("Challenge not found.");

            challenge.Rename(request.Name.Trim());

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}