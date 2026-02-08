using MediatR;

namespace Application.Features.AppIdeas.Commands
{
    public sealed record CreateAppIdeaCommand(
        string Type,
        string Category,
        string Description) : IRequest<CreateAppIdeaResult>;
}