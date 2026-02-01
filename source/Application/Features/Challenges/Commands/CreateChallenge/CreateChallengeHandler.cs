using Data.Models;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Challenges.Commands.CreateChallenge
{
    public sealed class CreateChallengeHandler : IRequestHandler<CreateChallengeCommand, CreateChallengeResult>
    {
        private readonly IAppDbContext _dbContext;

        public CreateChallengeHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateChallengeResult> Handle(CreateChallengeCommand request, CancellationToken cancellationToken)
        {
            // Pick a random AppIdea and Palette
            // For MVP simplicity, ORDER BY NEWID()
            var appIdea = await _dbContext.AppIdeas              
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefaultAsync(cancellationToken);

            var palette = await _dbContext.Palettes
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefaultAsync(cancellationToken);

            if(appIdea is null)
                throw new InvalidOperationException("Cannot create a challenge because no AppIdeas exist.");

            if(palette is null)
                throw new InvalidOperationException("Cannot create a challenge because no Palettes exist.");

            // Create a name, for MVP just Category and Type
            var challengeName = $"{appIdea.Category} {appIdea.Type} Challenge";

            var challenge = new Challenge(challengeName, appIdea, palette);

            _dbContext.Challenges.Add(challenge);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateChallengeResult(
                ChallengeId: challenge.Id,
                Name:  challenge.Name,
                AppIdeaId: appIdea.Id,
                PaletteId: palette.Id);
        }
    }
}