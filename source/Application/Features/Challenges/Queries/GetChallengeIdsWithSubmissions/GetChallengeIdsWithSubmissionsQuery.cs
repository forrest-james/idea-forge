using MediatR;

namespace Application.Features.Challenges.Queries.GetChallengeIdsWithSubmissions
{
    public sealed record GetChallengeIdsWithSubmissionsQuery : IRequest<HashSet<Guid>>;
}