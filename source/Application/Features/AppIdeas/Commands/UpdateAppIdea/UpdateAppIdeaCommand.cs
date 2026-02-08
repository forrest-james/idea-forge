using MediatR;

namespace Application.Features.AppIdeas.Commands.UpdateAppIdea
{
    public sealed record UpdateAppIdeaCommand(
        Guid Id,
        string Type,
        string Category,
        string Description
        ) : IRequest;
}