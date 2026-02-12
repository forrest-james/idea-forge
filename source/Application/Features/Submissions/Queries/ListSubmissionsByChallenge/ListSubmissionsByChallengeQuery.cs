using Application.Features.Submissions.DTOs;
using MediatR;

namespace Application.Features.Submissions.Queries.ListSubmissionsByChallenge
{
    public sealed record ListSubmissionsByChallengeQuery(Guid ChallengeId) : IRequest<IReadOnlyList<SubmissionListItemDto>>;
}