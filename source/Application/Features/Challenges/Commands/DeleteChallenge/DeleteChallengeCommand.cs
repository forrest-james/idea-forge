using MediatR;

namespace Application.Features.Challenges.Commands.DeleteChallenge
{
    public sealed record DeleteChallengeCommand(Guid Id) : IRequest;
}