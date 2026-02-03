using Application.Features.Challenges.DTOs;
using MediatR;

namespace Application.Features.Challenges.Queries.GetChallenge
{
    public sealed record GetChallengeQuery(Guid Id) : IRequest<ChallengeDto?>;
}