using Application.Features.AppIdeas.DTOs;
using MediatR;

namespace Application.Features.AppIdeas.Queries.GetAppIdea
{
    public sealed record GetAppIdeaQuery(Guid Id) : IRequest<AppIdeaDto?>;
}