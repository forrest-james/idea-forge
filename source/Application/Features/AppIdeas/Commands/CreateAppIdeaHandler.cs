using Data.Enums;
using Data.Models;
using Data.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AppIdeas.Commands
{
    public sealed class CreateAppIdeaHandler : IRequestHandler<CreateAppIdeaCommand, CreateAppIdeaResult>
    {
        private readonly IAppDbContext _dbContext;
        public CreateAppIdeaHandler(IAppDbContext dbContext) => _dbContext = dbContext;

        public async Task<CreateAppIdeaResult> Handle(CreateAppIdeaCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Description))
                throw new InvalidOperationException("Description is required.");

            if (!Enum.TryParse(request.Type, true, out AppType type))
                throw new InvalidOperationException($"Invalid Type '{request.Type}'.");

            if (!Enum.TryParse(request.Category, true, out AppCategory category))
                throw new InvalidOperationException($"Invalid Category '{request.Category}'.");

            var exists = await _dbContext.AppIdeas
                .AsNoTracking()
                .AnyAsync(a => a.Type == type && a.Category == category && a.Description == request.Description, cancellationToken);

            if (exists)
                throw new InvalidOperationException("An identical app idea already exists.");

            var appIdea = new AppIdea(type, category, request.Description.Trim());

            _dbContext.AppIdeas.Add(appIdea);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateAppIdeaResult(appIdea.Id);
        }
    }
}