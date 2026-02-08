using MediatR;

namespace Application.Features.AppIdeas.Commands.CreateAppIdea
{
    public sealed record CreateAppIdeaCommand(
        string Type,
        string Category,
        string Description) : IRequest<CreateAppIdeaResult>;
}