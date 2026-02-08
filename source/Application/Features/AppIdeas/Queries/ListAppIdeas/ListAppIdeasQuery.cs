using Application.Features.AppIdeas.DTOs;
using MediatR;

namespace Application.Features.AppIdeas.Queries.ListAppIdeas
{
    public sealed record ListAppIdeasQuery : IRequest<IReadOnlyList<AppIdeaDto>>;
}