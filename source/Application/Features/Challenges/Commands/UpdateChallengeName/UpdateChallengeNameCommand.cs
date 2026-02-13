using MediatR;

namespace Application.Features.Challenges.Commands.UpdateChallengeName
{
    public sealed record UpdateChallengeNameCommand(Guid Id, string Name) : IRequest;
}