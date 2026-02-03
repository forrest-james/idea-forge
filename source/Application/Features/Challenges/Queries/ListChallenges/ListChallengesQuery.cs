using Application.Features.Challenges.DTOs;
using MediatR;

namespace Application.Features.Challenges.Queries.ListChallenges
{
    public sealed record ListChallengesQuery : IRequest<IReadOnlyList<ChallengeListItemDto>>;
}