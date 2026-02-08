using MediatR;

namespace Application.Features.AppIdeas.Commands.DeleteAppIdea
{
    public sealed record DeleteAppIdeaCommand(Guid Id) : IRequest;
}