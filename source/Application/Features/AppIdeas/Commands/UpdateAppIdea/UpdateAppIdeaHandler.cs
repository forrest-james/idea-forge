using Data.Enums;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
`
namespace Application.Features.AppIdeas.Commands.UpdateAppIdea
{
    public sealed class UpdateAppIdeaHandler : IRequestHandler<UpdateAppIdeaCommand>
    {
        private readonly IAppDbContext _dbContext;
        public UpdateAppIdeaHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task Handle(UpdateAppIdeaCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new InvalidOperationException("Id is required.");

            if (string.IsNullOrWhiteSpace(request.Description))
                throw new InvalidOperationException("Description is required.");

            if (!Enum.TryParse(request.Type, true, out AppType type))
                throw new InvalidOperationException($"Invalid Type '{request.Type}'.");

            if (!Enum.TryParse(request.Category, true, out AppCategory category))
                throw new InvalidOperationException($"Invalid Category '{request.Category}'.");

            var appIdea = await _dbContext.AppIdeas.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
            if (appIdea is null)
                throw new InvalidOperationException("AppIdea not found.");

            appIdea.Update(type, category, request.Description.Trim());

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}