using MediatR;

namespace Application.Features.AppIdeas.Queries.GetAppIdeaIdsInUse
{
    public sealed record GetAppIdeaIdsInUseQuery : IRequest<HashSet<Guid>>;
}