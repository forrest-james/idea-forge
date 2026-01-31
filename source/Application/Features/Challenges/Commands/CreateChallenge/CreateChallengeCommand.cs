using MediatR;

namespace Application.Features.Challenges.Commands.CreateChallenge
{
    public sealed record CreateChallengeCommand : IRequest<CreateChallengeResult>
    {
    }
}