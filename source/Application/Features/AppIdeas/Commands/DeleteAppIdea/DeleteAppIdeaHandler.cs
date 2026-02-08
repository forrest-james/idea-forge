using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AppIdeas.Commands.DeleteAppIdea
{
    public sealed class DeleteAppIdeaHandler : IRequestHandler<DeleteAppIdeaCommand>
    {
        private readonly IAppDbContext _dbContext;
        public DeleteAppIdeaHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Handle(DeleteAppIdeaCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new InvalidOperationException("Id is required.");

            var inUse = await _dbContext.Challenges
                .AsNoTracking()
                .AnyAsync(c => EF.Property<Guid>(c, "AppIdeaId") == request.Id, cancellationToken);

            if (inUse)
                throw new InvalidOperationException("Cannot delete AppIdea because it is referenced by a Challenge.");

            var appIdea = await _dbContext.AppIdeas.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
            if (appIdea is null)
                return;

            _dbContext.AppIdeas.Remove(appIdea);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}